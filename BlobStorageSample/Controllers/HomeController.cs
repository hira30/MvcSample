using Azure.Storage.Blobs;
using BlobStorageSample.Models;
using Microsoft.AspNetCore.Mvc;

namespace BlobStorageSample.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class HomeController : Controller
    {
        private readonly string _connectionString = string.Empty;

        // �ݒ�t�@�C������X�g���[�W�A�J�E���g�̐ڑ���������擾
        public HomeController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("StorageAccount") 
                ?? throw new ArgumentNullException("�ڑ������񂪎w�肳��Ă��܂���B");
        }

        /// <summary>
        /// ��ʕ\��
        /// </summary>
        public IActionResult Index() => View();

        /// <summary>
        /// �摜�t�@�C�����A�b�v���[�h���ꂽ�ꍇ��Blob Storage�ɕۑ�����
        /// </summary>
        /// <param name="upload">�t�H�[������A�b�v���[�h���ꂽ�t�@�C��</param>
        [HttpPost]
        public async Task<IActionResult> Index([FromForm] UploadModel upload)
        {
            if (!ModelState.IsValid)
            {
                return View(upload);
            }

            // Blob Storage�̃R���e�i���i�m���Ɉ�ӂɂ��邽�߂�GUID��A���j
            var containerName = $"sample{Guid.NewGuid()}";

            try
            {
                // BlobServiceClient�N���X�̃C���X�^���X���쐬
                var blobServiceClient = new BlobServiceClient(_connectionString);

                // �R���e�i���쐬����
                BlobContainerClient containerClient = await blobServiceClient.CreateBlobContainerAsync(containerName);

                // BlobClient�N���X�̃C���X�^���X���擾
                var blobClient = containerClient.GetBlobClient(upload.File.FileName);

                // �t�@�C����BLOB�ɃA�b�v���[�h����
                await blobClient.UploadAsync(upload.File.OpenReadStream());

                ViewData["Result"] = "�t�@�C�����A�b�v���[�h���܂����I";
            }
            catch
            {
                ViewData["Result"] = "�t�@�C���̃A�b�v���[�h�Ɏ��s���܂����B";
            }

            return View();
        }
    }
}