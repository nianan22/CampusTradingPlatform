﻿using Autofac;
using System.Reflection;

namespace CampusTradingPlatform.AutofaceDI
{
    public class AutofacModuleRegister : Autofac.Module
    {
        // 重写Autoface中的Load方法，从而实现注入的操作
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            var AppServices = Assembly.Load("CTP.Service");
            var IAppServices = Assembly.Load("CTP.IService");
            var AppRepository = Assembly.Load("CTP.Repository");
            var IAppRepository = Assembly.Load("CTP.IRepository");
            // 这里做了一个约定，服务层中的类都是以Service结尾，仓储项目中的类都是以Repository结尾
            builder.RegisterAssemblyTypes(IAppServices, AppServices).Where(a => a.Name.EndsWith("Service")).AsImplementedInterfaces();//是以接口方式进行注入,

            builder.RegisterAssemblyTypes(IAppRepository, AppRepository).Where(a => a.Name.EndsWith("Repository")).AsImplementedInterfaces();
        }
    }
}
