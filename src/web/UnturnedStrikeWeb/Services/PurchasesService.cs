using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using UnturnedStrikeAPI;
using UnturnedStrikeDatabaseProvider.Repositories;

namespace UnturnedStrikeWeb.Services
{
    public class PurchasesService : IPurchasesService
    {
        private readonly IPlayersRepository playersRepository;
        private readonly IPurchasesRepository purchasesRepository;
        private readonly IConfiguration configuration;

        public PurchasesService(IPlayersRepository playersRepository, IPurchasesRepository purchasesRepository, 
            IConfiguration configuration)
        {
            this.playersRepository = playersRepository;
            this.purchasesRepository = purchasesRepository;
            this.configuration = configuration;
            VIPPrice = configuration.GetValue<decimal>("VIPPrice");
        }

        public decimal VIPPrice { get; }
    }
}
