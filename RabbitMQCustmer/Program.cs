﻿using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading.Channels;

namespace RabbitMQConsumer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to RabbitMQ Consumer1");
            TopicAcceptExchange();
            Console.WriteLine("按任意值，退出程序");
            Console.ReadKey();
        }

        /// <summary>
        /// 链接配置
        /// </summary>
        private static readonly ConnectionFactory rabbitMqFactory = new ConnectionFactory()
        {
            HostName = "127.0.0.1",
            UserName = "jinzazAdmin",
            Password = "123456",
            Port = 5672,
            VirtualHost = "jinzazVirtualHost"
        };

        /// <summary>
        /// 路由名称
        /// </summary>
        const string ExchangeName = "jinzaz.exchange";

        //队列名称
        const string QueueName = "jinzaz.queue";

        /// <summary>
        /// 路由名称
        /// </summary>
        const string TopExchangeName = "topic.jinzaz.exchange";

        //队列名称
        const string TopQueueName = "topic.jinzaz.queue";

        /// <summary>
        /// 基于时间轮询的，每隔一段时间获取一次
        /// </summary>
        public static void DirectAcceptExchange()
        {
            using (IConnection conn = rabbitMqFactory.CreateConnection())
            {
                using (IModel channel = conn.CreateModel())
                {
                    channel.ExchangeDeclare(ExchangeName,ExchangeType.Direct,durable:true,autoDelete:false,arguments:null);
                    channel.QueueDeclare(QueueName,durable:true,autoDelete:false,exclusive:false,arguments:null);
                    channel.QueueBind(QueueName,ExchangeName,routingKey:QueueName);
                    while (true)
                    {
                        BasicGetResult msgResponse = channel.BasicGet(QueueName,true);
                        if (msgResponse != null)
                        {
                            var msgBody = Encoding.UTF8.GetString(msgResponse.Body);
                            Console.WriteLine(string.Format("***接收时间：{0}，消息内容：{1}",DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),msgBody));
                        }

                        System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));
                    }
                }
            }
        }

        /// <summary>
        /// 基于事件的，当消息到达时触发事件，获取数据
        /// </summary>
        public static void DirectAcceptExchangeEvent()
        {
            using (IConnection conn = rabbitMqFactory.CreateConnection())
            {
                using (IModel channel = conn.CreateModel())
                {
                    channel.QueueDeclare(QueueName,durable:true,autoDelete:false,exclusive:false,arguments:null);
                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (model, ea) => {
                        var msgBody = Encoding.UTF8.GetString(ea.Body);
                        Console.WriteLine(string.Format("***接收时间：{0}，消息内容：{1}",DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),msgBody));
                    };
                    channel.BasicConsume(QueueName, true, consumer:consumer);
                    Console.WriteLine("按任意值，退出程序");
                    Console.ReadKey();
                }
            }
        }

        /// <summary>
        /// 基于事件的，当消息到达时触发事件
        /// </summary>
        public static void DirectAcceptExchangeTask()
        {
            using (IConnection conn = rabbitMqFactory.CreateConnection())
            {
                using (IModel channel = conn.CreateModel())
                {
                    channel.QueueDeclare(QueueName,durable:true,autoDelete:false,exclusive:false,arguments:null);
                    channel.BasicQos(prefetchSize:0,prefetchCount:1,global:false);
                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (model, ea) => {
                        var msgBody = Encoding.UTF8.GetString(ea.Body);
                        Console.WriteLine(string.Format("***接收事件：{0}，消息内容：{1}",DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),msgBody));
                        int dots = msgBody.Split('.').Length - 1;
                        System.Threading.Thread.Sleep(dots * 1000);
                        //处理完成，告诉Broker可以服务端删除消息，分配新的消息过来
                        channel.BasicAck(deliveryTag:ea.DeliveryTag,multiple:false);
                    };
                    //noAck设置fasle，告诉broker，发送消息后，消息暂时不要删除，等消费者处理完成再说
                    channel.BasicConsume(QueueName,false,consumer:consumer);
                    Console.WriteLine("按任意值，退出程序");
                    Console.ReadKey();
                }
            }
        }

        /// <summary>
        /// topic 模糊匹配模式，符号“#”匹配一个或多个词，符号“*”匹配不多不少一个词。因此“log.#”能够匹配到“log.info.oa”，但是“log.*”只会匹配到“log.error”
        /// </summary>

        public static void TopicAcceptExchange()
        {
            using (IConnection conn = rabbitMqFactory.CreateConnection())
            {
                using (IModel channel = conn.CreateModel())
                {
                    channel.ExchangeDeclare(TopExchangeName,ExchangeType.Topic,durable:false,autoDelete:false,arguments:null);
                    channel.QueueDeclare(TopExchangeName,  durable:false,autoDelete:false,exclusive:false,arguments:null);
                    channel.BasicQos(prefetchSize:0,prefetchCount:1,global:false);
                    channel.QueueBind(TopQueueName,TopExchangeName,routingKey:TopQueueName);
                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (model,ea) => {
                        var msgBody = Encoding.UTF8.GetString(ea.Body);
                        Console.WriteLine(string.Format("***接收时间：{0}，消息内容：{1}",DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),msgBody));
                        int dots = msgBody.Split('.').Length - 1;
                        System.Threading.Thread.SpinWait(dots * 1000);
                        Console.WriteLine("[x] Done");
                        channel.BasicAck(deliveryTag:ea.DeliveryTag,multiple:false);
                    };
                    channel.BasicConsume(TopQueueName,false,consumer:consumer);
                    Console.WriteLine("按任意值，退出程序");
                    Console.ReadKey();
                }
            }       
        }
    }
}
