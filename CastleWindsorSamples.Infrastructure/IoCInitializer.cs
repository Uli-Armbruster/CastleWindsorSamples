using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

using Castle.Core;
using Castle.Facilities.Startable;
using Castle.Facilities.TypedFactory;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.Windsor;
using Castle.Windsor.Installer;

namespace CastleWindsorSamples.Infrastructure
{
    public class IoCInitializer : IDisposable
    {
        // ReSharper disable once AssignNullToNotNullAttribute
        private static readonly DirectoryInfo RootFolder = new FileInfo(Assembly.GetExecutingAssembly().Location).Directory;

        private readonly IWindsorContainer _container;

        public IoCInitializer()
        {
            _container = new WindsorContainer();
        }

        public IoCInitializer RegisterComponents()
        {
            InstallContainerItself();

            Debug.WriteLine("Root-Folder for initializing IoC container: {0}", RootFolder.FullName);

            AddResolvers();

            AddFacilities();

            InstallFromAssemblies(ScanAssemblies());

            InstallFromExecutables(ScanExecutables());

            RegisterDaemons();

            return this;
        }

        private void AddFacilities()
        {
            _container.AddFacility<TypedFactoryFacility>();
            _container.AddFacility<StartableFacility>(f => f.DeferredStart());
        }

        public void AddResolvers()
        {
            _container.Kernel.Resolver.AddSubResolver(new CollectionResolver(_container.Kernel));
        }

        private void RegisterDaemons()
        {
            _container.Register(Classes
                                    .FromThisAssembly()
                                    .BasedOn<IStartable>()
                                    .WithService
                                    .DefaultInterfaces()
                                    .LifestyleSingleton()
                               );
        }

        public IWindsorContainer Instance()
        {
            Debug.WriteLine($"Folder scanned: {RootFolder.FullName}");
            Debug.WriteLine($"Number of registrations: {GetNumberOfRegistrations()}");

            return _container;
        }

        public IoCInitializer RegisterComponents(IWindsorInstaller[] installerForTestings)
        {
            if (installerForTestings == null ||
                !installerForTestings.Any())
            {
                return this;
            }

            _container.Install(installerForTestings);

            return this;
        }

        public void Dispose()
        {
            _container?.Dispose();
        }

        private static IEnumerable<IWindsorInstaller> ScanAssemblies()
        {
            return ScanFiles(AssembliesFilterPattern());
        }

        private static IEnumerable<IWindsorInstaller> ScanExecutables()
        {
            return ScanFiles(ExecutablesFilterPattern());
        }

        private static IEnumerable<IWindsorInstaller> ScanFiles(Func<FileInfo, bool> fileFilter)
        {
            var files = new List<FileInfo>();

            FoldersToScan()
                .ToList()
                .ForEach(
                         folder =>
                             files.AddRange(
                                            folder
                                                .GetFiles()
                                                .Where(fileFilter)
                                           )
                        );

            return files.Select(executable => FromAssembly.Named(executable.FullName));
        }

        private static Func<FileInfo, bool> ExecutablesFilterPattern()
        {
            return e =>
                e.Name.StartsWith("CastleWindsorSamples", StringComparison.OrdinalIgnoreCase)
                && !e.Name.ToLower().Contains("vshost")
                && e.Name.EndsWith(".exe", StringComparison.OrdinalIgnoreCase);
        }

        private static Func<FileInfo, bool> AssembliesFilterPattern()
        {
            return e =>
                e.Name.StartsWith("CastleWindsorSamples", StringComparison.OrdinalIgnoreCase)
                && e.Name.EndsWith(".dll", StringComparison.OrdinalIgnoreCase);
        }

        private void InstallFromExecutables(IEnumerable<IWindsorInstaller> installer)
        {
            Install(installer);
        }

        private void InstallFromAssemblies(IEnumerable<IWindsorInstaller> installer)
        {
            Install(installer);
        }

        private void Install(IEnumerable<IWindsorInstaller> installer)
        {
            var all = installer.ToArray();
            _container.Install(all);
        }

        private void InstallContainerItself()
        {
            _container.Register(Component.For<IWindsorContainer>().Instance(_container));
        }

        private static IEnumerable<DirectoryInfo> FoldersToScan()
        {
            yield return RootFolder;
        }

        private int GetNumberOfRegistrations()
        {
            return _container.Kernel.GetAssignableHandlers(typeof(object)).Length;
        }
    }
}
