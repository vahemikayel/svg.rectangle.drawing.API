using MediatR;
using Microsoft.Extensions.Options;
using SVG.API.Models.Response.Data;
using SVG.API.Infrastructure.Options;
using AutoMapper;
using SVG.API.Models.Entity;
using Newtonsoft.Json;

namespace SVG.API.Application.Queries.Data
{
    public class FileGetQuery : BaseRequest<FileReadModel>
    {
    }

    public class FileGetQueryHandler : IRequestHandler<FileGetQuery, FileReadModel>
    {
        private readonly IMapper _mapper;
        private readonly FileStorageOption _fileOption;

        public FileGetQueryHandler(IOptions<FileStorageOption> options, IMapper mapper)
        {
            _fileOption = options?.Value ?? throw new ArgumentNullException(nameof(options));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<FileReadModel> Handle(FileGetQuery request, CancellationToken cancellationToken)
        {
            var systemPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            var complete = Path.Combine(systemPath, _fileOption.Path);

            if (!Directory.Exists(complete))
                Directory.CreateDirectory(complete);

            var file = Path.Combine(complete, _fileOption.FileName);
            if (!File.Exists(file))
                using (var wr = File.CreateText(file))
                {
                    var fm = new FileDataModel() 
                    { 
                        Width = 300,
                        Height = 300 
                    };
                    var sfm = JsonConvert.SerializeObject(fm);
                    await wr.WriteLineAsync(sfm);
                    return _mapper.Map<FileReadModel>(fm);
                }

            using (var read = File.OpenText(file))
            {
                var data = await read.ReadToEndAsync();
                var desData = JsonConvert.DeserializeObject<FileDataModel>(data);
                return _mapper.Map<FileReadModel>(desData);
            }
        }
    }
}
