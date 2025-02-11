using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Laba7_2._0
{
    internal class Program
    {
        //Переменный для отслеживания состояния потоков
        static bool thread1Running = false;
        static bool thread2Running = false;

        // Объект для синхронизации доступа к консоли и к состоянию потоков
        static readonly object locker = new object();


        static void Main(string[] args)
        {
            //Переменные для хранения экземпляров потоков
            Thread thread1 = null;
            Thread thread2 = null;

            //Менюшка
            Console.WriteLine("Меню:");
            Console.WriteLine("1. Запустить поток 1");
            Console.WriteLine("2. Остановить поток 1");
            Console.WriteLine("3. Возобновить поток 1");
            Console.WriteLine("4. Запустить поток 2");
            Console.WriteLine("5. Остановить поток 2");
            Console.WriteLine("6. Возобновить поток 2");
            Console.WriteLine("Q/q. Выйти");

            while (true)
            {
                //Считывание клавишь
                char choice = Console.ReadKey().KeyChar;
                Console.WriteLine();

                //Управление
                switch (choice)
                {
                    case '1'://Запуск потока 1
                        StartThread(ref thread1, Thread1Function, ref thread1Running);
                        break;
                    case '2'://Остановка потока 1
                        StopThread(thread1, ref thread1Running);
                        break;
                    case '3'://Возобновление потока 1
                        ResumeThread(ref thread1, Thread1Function, ref thread1Running);
                        break;
                    case '4'://Запуск потока 2
                        StartThread(ref thread2, Thread2Function, ref thread2Running);
                        break;
                    case '5'://Остановка потока 2
                        StopThread(thread2, ref thread2Running);
                        break;
                    case '6'://Возобновление потока 1
                        ResumeThread(ref thread2, Thread2Function, ref thread2Running);
                        break;
                    case 'q'://Выход
                        return;
                    case 'Q'://Выход
                        return;
                    default://Случай нажатия какой то другой клавиши
                        Console.WriteLine("Неверный выбор.");
                        break;
                }
            }
        }

        //Функция, которая запускает поток, если он еще не запущен
        static void StartThread(ref Thread thread, ThreadStart threadStart, ref bool runningFlag)
        {
            lock (locker)
            {
                if (!runningFlag)
                {
                    try
                    {
                        thread = new Thread(threadStart);
                        thread.Start();
                        runningFlag = true;
                    }
                    catch (ThreadStateException ex)
                    {
                        Console.WriteLine($"Ошибка при запуске потока: {ex.Message}");
                    }
                }
                else
                {
                    Console.WriteLine("Поток уже запущен.");
                }
            }
        }

        //Функция, которая останавливает поток
        static void StopThread(Thread thread, ref bool runningFlag)
        {
            lock (locker)
            {
                if (runningFlag)
                {
                    thread.Abort();
                    runningFlag = false;
                }
                else
                {
                    Console.WriteLine("Поток уже остановлен.");
                }
            }
        }

        //Функция, которая возобновляет работу остановленного потока
        static void ResumeThread(ref Thread thread, ThreadStart threadStart, ref bool runningFlag)
        {
            lock (locker)
            {
                if (!runningFlag)
                {
                    thread = new Thread(threadStart);
                    thread.Start();
                    runningFlag = true;
                }
                else
                {
                    Console.WriteLine("Поток уже запущен.");
                }
            }
        }

        //Метод, который представляет функциональность первого потока
        static void Thread1Function()
        {
            while (true)
            {
                lock (locker)
                {
                    Console.WriteLine("Поток 1: " + DateTime.Now);
                }
                Thread.Sleep(1000);
            }
        }

        //Метод, который представляет функциональность второго потока
        static void Thread2Function()
        {
            while (true)
            {
                lock (locker)
                {
                    Console.WriteLine("Поток 2: " + DateTime.Now);
                }
                Thread.Sleep(1000);
            }
        }
    }
}
