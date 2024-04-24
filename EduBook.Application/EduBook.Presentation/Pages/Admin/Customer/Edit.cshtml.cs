﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EduBook.BusinessObject;
using EduBook.Service.IService;

namespace EduBook.Presentation.Pages.Admin.Customer
{
    public class EditModel : PageModel
    {
        private readonly IAccountService _accountService;
        private readonly ISlotService _slotService;
        private readonly IDepartmentService _departmentService;

        public EditModel(IAccountService accountService, 
                        ISlotService slotService, 
                        IDepartmentService departmentService)
        {
            _accountService = accountService;
            _slotService = slotService;
            _departmentService = departmentService;
        }

        [BindProperty]
        public Account Account { get; set; } = default!;

        public IActionResult OnGet(int? id)
        {
            var role = HttpContext.Session.GetInt32("role");
            if (role != 1)
            {
                return Redirect("/Customer/CustomerHomePage");
            }
            if (id == null)
            {
                return NotFound();
            }

            var account =  _accountService.GetById((int)id);
            if (account == null)
            {
                return NotFound();
            }
            Account = account;
           ViewData["DepartmentId"] = new SelectList(_departmentService.GetList(), "DepartmentId", "Address");
           ViewData["RoleId"] = new SelectList(_departmentService.GetList(), "RoleId", "RoleName");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _accountService.Update(Account);

            return RedirectToPage("./Index");
        }
    }
}
