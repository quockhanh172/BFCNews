﻿using System.Net.Mail;
using System.Net;

namespace BFCNews.Service
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string htmlMessage);
    }
}
