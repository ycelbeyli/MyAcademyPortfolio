﻿using Microsoft.AspNetCore.Mvc;

namespace Portfolio.Web.ViewComponents.Default_Index
{
    public class _DefaultFooterComponent:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
