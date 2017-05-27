using System.Web;
using NHibernate;
using NHibernate.Cfg;

namespace NhibernateMVC.Models
{
    public class NHibertnateSession
    {
        public static ISession OpenSession()
        {
            var configuration = new Configuration();
            var configurationPath = HttpContext.Current.Server.MapPath(@"~\Models\Nhibernate\hibernate.cfg.xml");
            configuration.Configure(configurationPath);
            var groupsConfigurationFile = HttpContext.Current.Server.MapPath(@"~\Models\Nhibernate\Groups.hbm.xml");
            var studentConfigurationFile = HttpContext.Current.Server.MapPath(@"~\Models\Nhibernate\Student.hbm.xml");
            var subjectsConfigurationFile = HttpContext.Current.Server.MapPath(@"~\Models\Nhibernate\Subjects.hbm.xml");
            var groupsSubjectsConfigurationFile = HttpContext.Current.Server.MapPath(@"~\Models\Nhibernate\Groups_Subjects.hbm.xml");
            var markConfigurationFile = HttpContext.Current.Server.MapPath(@"~\Models\Nhibernate\Marks.hbm.xml");
            configuration.AddFile(groupsConfigurationFile);
            configuration.AddFile(studentConfigurationFile);
            configuration.AddFile(subjectsConfigurationFile);
            configuration.AddFile(markConfigurationFile);
            configuration.AddFile(groupsSubjectsConfigurationFile);
            ISessionFactory sessionFactory = configuration.BuildSessionFactory();
            return sessionFactory.OpenSession();
        }
    }
}