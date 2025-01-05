using AutoMapper;
using Demo.DAL.Models;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{
	[Authorize]
	public class UserrController : Controller
	{
		private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public UserrController(UserManager<ApplicationUser> userManager,
			IMapper mapper)
		{
			_userManager = userManager;
            _mapper = mapper;
        }
		public async Task<IActionResult> Index(string SearchValue)
		{
			if (string.IsNullOrEmpty(SearchValue))
			{
				var user = await _userManager.Users.Select(
					U => new UserViewModel()
					{
						Id = U.Id,
						FName = U.FName,
						LName = U.LName,
						Email = U.Email,
						PhoneNumber = U.PhoneNumber,
						Roles = _userManager.GetRolesAsync(U).Result
					}).ToListAsync();
				return View(user);
			}
			else
			{
				var user = await _userManager.FindByEmailAsync(SearchValue);
				var mappedUser = new UserViewModel()
				{
					Id = user.Id,
					FName = user.FName,
					LName = user.LName,
					Email = user.Email,
					PhoneNumber = user.PhoneNumber,
					Roles = _userManager.GetRolesAsync(user).Result
				};
				return View(new List<UserViewModel> { mappedUser });
			}

		}
		//Details
		public async Task<IActionResult> Details(string id,string ViewName="Details")
		{
			if (id is null)
				return BadRequest();
			var user=await _userManager.FindByIdAsync(id);
			if (user == null)
				return NotFound();	
			var mappeduser=_mapper.Map<ApplicationUser,UserViewModel>(user);
			return View(ViewName,mappeduser);
		}

		//Edit
		public async Task<IActionResult> Edit(string id)
		{
			return await Details(id,"Edit");
		}
		[HttpPost]
		public async Task<IActionResult>Edit(UserViewModel model,[FromRoute]string id)
		{
			if(id !=model.Id)
				return BadRequest();
			if (ModelState.IsValid)
			{
				try
				{
					var User=await _userManager.FindByIdAsync(id);
					User.FName = model.FName;
					User.LName = model.LName;
					User.PhoneNumber = model.PhoneNumber;
					//var mappeduser=_mapper.Map<UserViewModel,ApplicationUser>(model);
					await _userManager.UpdateAsync(User);
					return RedirectToAction("Index");
				}catch(Exception ex)
				{
					ModelState.AddModelError(string.Empty,ex.Message);	
				}
			}
			return View(model);
		}
		//Delete
		public async  Task<IActionResult> Delete(string id)
		{
			return await Details(id, "Delete");
		}
		[HttpPost]
		public async Task<IActionResult> ConfirmDelete(string id)
		{
			try
			{
				var user=await _userManager.FindByIdAsync(id);
				await _userManager.DeleteAsync(user);
				return RedirectToAction("Index");	
			}catch(Exception ex)
			{
				ModelState.AddModelError(string.Empty, ex.Message);
				return RedirectToAction("Error", "Home");
			}
		}

    }
}
