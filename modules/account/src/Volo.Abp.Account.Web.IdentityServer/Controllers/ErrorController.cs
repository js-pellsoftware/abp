﻿using System.Threading.Tasks;
using AutoMapper.Internal;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Views.Error;
using Volo.Abp.Http;

namespace Volo.Abp.Account.Web.Controllers
{
    [Area("account")]
    public class ErrorController : AbpController
    {
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IWebHostEnvironment _environment;
        private readonly AbpErrorPageOptions _abpErrorPageOptions;

        public ErrorController(
            IIdentityServerInteractionService interaction,
            IWebHostEnvironment environment,
            IOptions<AbpErrorPageOptions> abpErrorPageOptions)
        {
            _interaction = interaction;
            _environment = environment;
            _abpErrorPageOptions = abpErrorPageOptions.Value;
        }

        public async Task<IActionResult> Index(string errorId)
        {
            var errorMessage = await _interaction.GetErrorContextAsync(errorId) ?? new ErrorMessage
            {
                Error = L["Error"]
            };

            if (!_environment.IsDevelopment())
            {
                // Only show in development
                errorMessage.ErrorDescription = null;
            }

            return View(GetErrorPageUrl(500), new AbpErrorViewModel
            {
                ErrorInfo = new RemoteServiceErrorInfo(errorMessage.Error, errorMessage.ErrorDescription),
                HttpStatusCode = 500
            });
        }

        private string GetErrorPageUrl(int statusCode)
        {
            var page = _abpErrorPageOptions.ErrorViewUrls.GetOrDefault(statusCode.ToString());

            return string.IsNullOrWhiteSpace(page) ? "~/Views/Error/Default.cshtml" : page;
        }
    }
}
