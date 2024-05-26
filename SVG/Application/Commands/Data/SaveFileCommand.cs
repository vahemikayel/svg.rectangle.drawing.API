using AutoMapper;
using MediatR;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SVG.API.Infrastructure.BaseReuqestTypes;
using SVG.API.Infrastructure.Options;
using SVG.API.Models.Entity;

namespace SVG.API.Application.Commands.Data
{
    public class SaveFileCommand : BaseRequest<bool>
    {
        //public string FileData { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }
    }

    public class SaveFileCommandHandler : IRequestHandler<SaveFileCommand, bool>
    {
        private readonly IMapper _mapper;
        private readonly FileStorageOption _fileOption;

        public SaveFileCommandHandler(IOptions<FileStorageOption> options, IMapper mapper)
        {
            _fileOption = options?.Value ?? throw new ArgumentNullException(nameof(options));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<bool> Handle(SaveFileCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var systemPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
                var complete = Path.Combine(systemPath, _fileOption.Path);

                if (!Directory.Exists(complete))
                    Directory.CreateDirectory(complete);

                var file = Path.Combine(complete, _fileOption.FileName);
                using (var wr = File.CreateText(file))
                {
                    var fData = _mapper.Map<FileDataModel>(request);
                    var sfm = JsonConvert.SerializeObject(fData);
                    await wr.WriteLineAsync(sfm);
                    return true;
                }
            }
            catch (Exception ex)
            {
                //LOG
                return false;
            }
        }
    }
}
