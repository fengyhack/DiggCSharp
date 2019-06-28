using System;
using System.Configuration;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;

namespace DemoUnityIoc
{
    /// <summary>
    /// IOC使用示例
    /// </summary>
    class IocSampleUsage
    {
        static void Main(string[] args)
        {
            var robot1 = SuperIoc.GetInstanceDAL<IRobot>();
            robot1.Move(100);

            var robot2 = SuperIoc.GetInstance().Container.Resolve<IRobot>();
            robot2.Move(200);

            var controller = SuperIoc.GetInstanceDAL<DeviceController>();
            controller.DoWork(300);
            controller.Robot.Move(400);

            // 在Unity配置文件中，如果设置了lifetime类型为singleton
            // 则以下结果均为true，否则均为false
            bool b1 = robot1.Equals(robot2);
            bool b2 = robot1.Equals(controller.Robot);
            Console.WriteLine($"robot1 Equals robot2 : {b1}");
            Console.WriteLine($"robot1 Equals robot3 : {b2}");

            Console.ReadKey();
        }
    }

    /// <summary>
    /// Robot接口
    /// </summary>
    public interface IRobot
    {
        void Move(int position);
    }

    /// <summary>
    /// A类型的Robot，实现IRobot接口
    /// </summary>
    public class AxRobot:IRobot
    {
        public void Move(int position)
        {
            Console.WriteLine($"AX move({position})");
        }
    }

    /// <summary>
    /// B类型的Robot，实现IRobot接口
    /// </summary>
    public class BxRobot : IRobot
    {
        public void Move(int position)
        {
            Console.WriteLine($"BX move({position})");
        }
    }

    /// <summary>
    /// 设备控制器，包含Robot等设备
    /// </summary>
    public class DeviceController
    {
        /// <summary>
        /// 添加[Dependency]标记后，自动完成依赖注入
        /// </summary>
        //[Dependency]
        public IRobot Robot { get; private set; }

        /// <summary>
        /// 构造函数，自动完成传入参数
        /// </summary>
        public DeviceController(IRobot robot_)
        {
            Robot = robot_;
        }

        /// <summary>
        /// 添加[Dependency]标记后，自动完成方法注入
        /// </summary>
        /// <param name="robot_"></param>
        //[InjectionMethod]
        public void Initialize(IRobot robot_)
        {
            Robot = robot_;
        }

        /// <summary>
        /// 控制内部设备(Robot)完成一系列工作
        /// </summary>
        public void DoWork(int position)
        {
            Robot.Move(position);
        }
    }

    /// <summary>
    /// IOC
    /// </summary>
    public class SuperIoc
    {
        public IUnityContainer Container { get; set; } = null;

        private static SuperIoc instance = null;

        private SuperIoc()
        {
            Container = null;
        }

        /// <summary>
        /// 单例,静态方法
        /// </summary>
        public static SuperIoc GetInstance()
        {
            if (instance == null || instance.Container == null)
            {                
                instance = new SuperIoc() { Container = GetContainer() };
            }

            return instance;
        }

        /// <summary>
        /// 从配置文件构建UnityContainer
        /// </summary>
        /// <param name="configFile">Unity配置文件名</param>
        /// <param name="sectionName">UnitySection名称</param>
        /// <param name="containerName">UnityContainer名称</param>
        private static IUnityContainer GetContainer(
            string configFile = "unity.config",
            string sectionName = "superSection",
            string containerName = "superContainer")
        {
            var container = new UnityContainer();
            var fileMap = new ExeConfigurationFileMap() { ExeConfigFilename = configFile };
            var configuration = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
            var unitySection = (UnityConfigurationSection)configuration.GetSection(sectionName);
            container.LoadConfiguration(unitySection, containerName);

            return container;
        }

        /// <summary>
        /// 根据类型构建对象
        /// </summary>
        /// <typeparam name="T">目标对象类型</typeparam>
        /// <returns>构建的目标</returns>
        public static T GetInstanceDAL<T>() 
            where T : class
        {
            return GetInstance().Container.Resolve<T>();
        }
    }
}
