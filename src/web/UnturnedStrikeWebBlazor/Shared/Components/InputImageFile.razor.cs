using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System;
using System.Net.Http.Headers;

namespace UnturnedStrikeWebBlazor.Shared.Components
{
    public partial class InputImageFile
    {
        [Parameter]
        public int? FileId { get; set; }
        [Parameter]
        public EventCallback<int?> FileIdChanged { get; set; }

        [Parameter]
        public string Accept { get; set; }

        [Inject]
        private HttpClient HttpClient { get; set; }

        private async Task HandleChange(InputFileChangeEventArgs args)
        {
            IBrowserFile file = args.File;
            file = await file.RequestImageFileAsync("image/png", 800, 800);

            HttpRequestMessage msg = new(HttpMethod.Post, $"api/files")
            {
                Content = new MultipartFormDataContent()
            };

            var content = new StreamContent(file.OpenReadStream(30 * 1024 * 1024));
            content.Headers.ContentType = MediaTypeHeaderValue.Parse(file.ContentType);
            (msg.Content as MultipartFormDataContent).Add(content, "file", file.Name);
            var response = await HttpClient.SendAsync(msg);

            if (response.IsSuccessStatusCode)
            {
                FileId = Convert.ToInt32(await response.Content.ReadAsStringAsync());
                await FileIdChanged.InvokeAsync(FileId);
            }
        }
    }
}
