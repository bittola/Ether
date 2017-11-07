﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Ether.Core.Interfaces;
using Ether.Models;
using Ether.Core.Models.DTO;
using Ether.Jobs;
using FluentScheduler;
using Ether.Extensions;

namespace Ether.Pages.Settings
{
    public class TeamMembersModel : PageModel
    {
        private readonly IRepository _repository;

        public TeamMembersModel(IRepository repository)
        {
            _repository = repository;
        }

        [BindProperty(SupportsGet = true)]
        public IEnumerable<string> Teams { get; set; }

        public IEnumerable<string> AllTeams { get; set; }

        public IEnumerable<TeamMember> TeamMembers { get; set; }

        public async Task OnGet()
        {
            var members = (await _repository.GetAllAsync<TeamMember>())
                .OrderBy(m => m.TeamName);
            AllTeams = members.Select(m => m.TeamName)
                .Distinct();

            if (Teams == null || !Teams.Any())
            {
                Teams = Enumerable.Empty<string>();
                TeamMembers = members;
            }
            else
            {
                TeamMembers = members.Where(m => Teams.Contains(m.TeamName));
            }
        }

        public async Task<IActionResult> OnPostDeleteAsync(Guid id)
        {
            await _repository.DeleteAsync<TeamMember>(id);
            return RedirectToPage("TeamMembers");
        }

        public async Task<IActionResult> OnPostResetWorkitemsAsync(Guid id)
        {
            var user = await _repository.GetSingleAsync<TeamMember>(t => t.Id == id);
            if (user != null)
            {
                user.LastFetchDate = DateTime.MinValue;
                user.RelatedWorkItemIds = Enumerable.Empty<int>();
                await _repository.CreateOrUpdateAsync(user);
                var job = HttpContext.RequestServices.GetService(typeof(WorkItemsFetchJob)) as WorkItemsFetchJob;
                if (job != null)
                {
                    job.SpecificUser = user;
                    JobManager.AddJob(job, s => s.ToRunNow());
                }
                TempData.WithSuccess($"Work items for {user.DisplayName} have been cleared.");
            }

            return RedirectToPage("TeamMembers");
        }
    }
}