using AngleSharp.Common;
using Discord;
using Discord.Webhook;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SteamWebAPI2.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using UnturnedStrikeAPI;
using UnturnedStrikeDatabaseProvider.Repositories;

namespace UnturnedStrikeWeb.Services
{
    public class TransactionsService : ITransactionsService
    {
        private readonly ILogger<TransactionsService> logger;
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IConfiguration configuration;
        private readonly ITransactionsRepository purchasesRepository;
        private readonly IPlayersRepository playersRepository;

        public string PayPalUrl { get; }
        public string PayPalEmail { get; }
        public string Currency { get; set; }

        public TransactionsService(ILogger<TransactionsService> logger, IHttpClientFactory httpClientFactory, IConfiguration configuration, ITransactionsRepository purchasesRepository, 
            IPlayersRepository playersRepository)
        {
            this.logger = logger;
            this.httpClientFactory = httpClientFactory;
            this.configuration = configuration;
            this.purchasesRepository = purchasesRepository;
            this.playersRepository = playersRepository;
            PayPalUrl = configuration["PayPalUrl"];
            PayPalEmail = configuration["PayPalEmail"];

            Currency = configuration["Currency"];
        }

        public async Task PayPalVerifyAsync(HttpRequest request)
        {
            string requestBody;
            using (var reader = new StreamReader(request.Body, Encoding.ASCII))
            {
                requestBody = await reader.ReadToEndAsync();
            }

            var content = new StringContent("cmd=_notify-validate&" + requestBody);

            var response = await httpClientFactory.CreateClient().PostAsync(PayPalUrl, content);
            var verification = await response.Content.ReadAsStringAsync();
            
            if (verification.Equals("VERIFIED"))
            {
                await ProcessPayPalTransactionAsync(HttpUtility.ParseQueryString(requestBody));
            }
        }

        private async Task ProcessPayPalTransactionAsync(NameValueCollection dict)
        {
            if (dict["payment_status"] != "Completed" || dict["receiver_email"] != PayPalEmail)
            {
                return;
            }

            var transaction = new Transaction() 
            { 
                PlayerId = dict["custom"],
                Email = dict["payer_email"],
                TransactionId = dict["txn_id"],
                Gross = Convert.ToDecimal(dict["mc_gross"]),
                Fee = Convert.ToDecimal(dict["mc_fee"]),
                Currency = dict["mc_currency"]
            };

            transaction = await purchasesRepository.LogTransactionAsync(transaction);
            transaction.Player = await playersRepository.GetPlayerAsync(transaction.PlayerId);

            if (transaction.Currency == Currency)
            {
                await playersRepository.AddToPlayerBalanceAsync(transaction.PlayerId, transaction.Gross);
            }
                        
            logger.LogInformation($"Successfully processed the purchase from {transaction.Email} for {transaction.PlayerId}!");
            await Task.Run(() => LogToDiscordAsync(transaction));
        }

        private async Task LogToDiscordAsync(Transaction transaction)
        {
            var eb = new EmbedBuilder();
            eb.WithColor(Color.Green);
            eb.WithTitle(transaction.Email);
            eb.AddField("Gross", $"{transaction.Gross} {transaction.Currency}", true);
            eb.AddField("Fee", $"{transaction.Fee} {transaction.Currency}", true);
            eb.AddField("SteamName", $"{transaction.Player.SteamName}", true);
            eb.AddField("SteamID", $"{transaction.PlayerId}", true);
            using (var client = new DiscordWebhookClient(configuration["PaymentsWebhookUrl"]))
            {
                await client.SendMessageAsync(embeds: new Embed[] { eb.Build() });
            }
        }
    }
}
