using Azure.Storage.Blobs;
using BlobStorageSample.Models;
using Microsoft.AspNetCore.Mvc;

namespace BlobStorageSample.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class HomeController : Controller
    {
        private readonly string _connectionString = string.Empty;

        // 設定ファイルからストレージアカウントの接続文字列を取得
        public HomeController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("StorageAccount") 
                ?? throw new ArgumentNullException("接続文字列が指定されていません。");
        }

        /// <summary>
        /// 画面表示
        /// </summary>
        public IActionResult Index() => View();

        /// <summary>
        /// 画像ファイルがアップロードされた場合はBlob Storageに保存する
        /// </summary>
        /// <param name="upload">フォームからアップロードされたファイル</param>
        [HttpPost]
        public async Task<IActionResult> Index([FromForm] UploadModel upload)
        {
            if (!ModelState.IsValid)
            {
                return View(upload);
            }

            // Blob Storageのコンテナ名（確実に一意にするためにGUIDを連結）
            var containerName = $"sample{Guid.NewGuid()}";

            try
            {
                // BlobServiceClientクラスのインスタンスを作成
                var blobServiceClient = new BlobServiceClient(_connectionString);

                // コンテナを作成する
                BlobContainerClient containerClient = await blobServiceClient.CreateBlobContainerAsync(containerName);

                // BlobClientクラスのインスタンスを取得
                var blobClient = containerClient.GetBlobClient(upload.File.FileName);

                // ファイルをBLOBにアップロードする
                await blobClient.UploadAsync(upload.File.OpenReadStream());

                ViewData["Result"] = "ファイルをアップロードしました！";
            }
            catch
            {
                ViewData["Result"] = "ファイルのアップロードに失敗しました。";
            }

            return View();
        }
    }
}