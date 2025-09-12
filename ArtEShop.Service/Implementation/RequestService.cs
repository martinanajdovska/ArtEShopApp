using ArtEShop.Domain.DomainModels;
using ArtEShop.Domain.DTO;
using ArtEShop.Domain.Email;
using ArtEShop.Repository.Interface;
using ArtEShop.Service.Interface;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ArtEShop.Service.Implementation
{
    public class RequestService : IRequestService
    {
        private readonly IRepository<Request> _requestRepository;
        private readonly IEmailService _emailService;
        private readonly MailSettings _mailSettings;

        public RequestService(IRepository<Request> requestRepository, IEmailService emailService, IOptions<MailSettings> mailSettings)
        {
            _requestRepository = requestRepository;
            _emailService = emailService;
            _mailSettings = mailSettings.Value;
        }

        public List<Request>? GetAllByUser(string email)
        {
            return _requestRepository.GetAll(selector: x => x, predicate: x => x.Email.Equals(email)).ToList();
        }

        public Request? GetById(Guid id)
        {
            return _requestRepository.Get(selector: x => x,
                                          predicate: x => x.Id.Equals(id));
        }

        public List<Request> GetAll()
        {
            return _requestRepository.GetAll(selector: x => x).ToList();
        }
        public async Task<bool> RequestPiece(CreateRequestDTO model, string fileName)
        {

            Request request = new Request
            {
                Id = Guid.NewGuid(),
                Description = model.Description,
                ReferenceImage = fileName,
                Price = null,
                ArtistNotes = null,
                IsAnswered = false,
                Email = model.Email,
                Subject = model.Subject,
            };

            _requestRepository.Insert(request);

            var emailMessage = new EmailMessage();

            emailMessage.MailTo = model.Email;
            emailMessage.Subject = "Successfull order";

            emailMessage.Content = "The artist has received your commission. Additional information will be provided.";

            await _emailService.SendEmailAsync(emailMessage);
            return true;
        }

        public async Task<Request> UpdateFromDTO(RequestDTO requestDTO, bool artistUpdated)
        {
            Request request = GetById(requestDTO.RequestId);

            request.IsAnswered = artistUpdated;

            var emailMessage = new EmailMessage();
            emailMessage.MailTo = request.Email;

            if (artistUpdated)
            {
                request.ArtistNotes = requestDTO.ArtistNotes;
                request.Price = requestDTO.Price;

                emailMessage.Subject = "Request Update";

                emailMessage.Content = $"The artist has left some feedback on your request.\n" +
                    $"Artist note: {request.ArtistNotes}\n" +
                    $"Price: {request.Price}\n" +
                    $"Contact the artist at: {_mailSettings.SmtpUserName}";
            }
            else
            {
                request.Subject = requestDTO.Subject;
                request.Description = requestDTO.Description;
                emailMessage.Subject = "Request Update";
                emailMessage.Content = "The artist has received you updated request.";
            }
            _requestRepository.Update(request);
            await _emailService.SendEmailAsync(emailMessage);

            return request;
        }

        public RequestDTO EntityToDTO(Request request)
        {
            RequestDTO model = new RequestDTO
            {
                RequestId = request.Id,
                Subject = request.Subject,
                Description = request.Description,
                Price = request.Price,
                ReferenceImage = request.ReferenceImage,
                ArtistNotes = request.ArtistNotes,
                IsAnswered = request.IsAnswered
            };

            return model;
        }

    }
}
