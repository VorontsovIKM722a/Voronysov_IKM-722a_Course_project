﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using System.Diagnostics;
using System.Collections;
namespace Voronysov_IKM_722a_Course_project_
{
    internal class MajorWork
    {// Вміст робочого об'єкта
     // Поля
        private System.DateTime TimeBegin; // час початку роботи програми
        private string Data; //вхідні дані
        private string Result; // Поле результату
        public bool Modify;
        private int Key;// поле ключа

        public Stack myStack = new Stack();
        public string[] myArr = new string[100];

        public Queue myQueue = new Queue();
        public string[] smyQueue = new string[100];
        private string SaveTextFileName;// ім'я файлу для запису текстового файлу

        private string OpenTextFileName;
        public void WriteOpenTextFileName(string S)
        {
            this.OpenTextFileName = S;
        }
        public void WriteSaveTextFileName(string S)
        {
            this.SaveTextFileName = S;
        }

        // Методи
        private string SaveFileName;// ім’я файлу для запису
        private string OpenFileName;// ім’я файлу для читання

        public object D { get; private set; }

        public void WriteSaveFileName(string S)// метод запису даних в об'єкт
        {
            this.SaveFileName = S;// запам'ятати ім’я файлу для запису
        }
        public void WriteOpenFileName(string S)
        {
            this.OpenFileName = S;// запам'ятати ім’я файлу для відкриття
        }
        public void SetTime() // метод запису часу початку роботи програми
        {
            this.TimeBegin = System.DateTime.Now;

        }
        public System.DateTime GetTime() // Метод отримання часу завершення програми
        {
            return this.TimeBegin;
        }
        public void Write(string D)// метод запису даних в об'єкт.
        {
            this.Data = D;
        }
        public string Read()
        {
            return this.Result;// метод відображення результату
        }
        public void Task()
        {
            
            
                string[] dataParts = Data.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);

                int sum = 0;


                int[] NData = new int[dataParts.Length];
                for (int i = 0; i < dataParts.Length; i++)
                {
                    NData[i] = int.Parse(dataParts[i]);
                }


                for (int i = 0; i < dataParts.Length; i += 2)
                {
                    sum += NData[i];
                }
                this.Result = "Cума кожного непарного елементу :" + sum.ToString();
                this.Modify = true;
        }
        public void SaveToFile() // Запис даних до файлу
        {
            if (!this.Modify)
                return;
            try
            {
                Stream S; // створення потоку
                if (File.Exists(this.SaveFileName))// існує файл?
                    S = File.Open(this.SaveFileName, FileMode.Append);// Відкриття файлу для збереження
                else
                    S = File.Open(this.SaveFileName, FileMode.Create);// створити файл
                Buffer D = new Buffer(); // створення буферної змінної
                D.Data = this.Data;
                D.Result = Convert.ToString(this.Result);
                D.Key = Key;
                Key++;
                BinaryFormatter BF = new BinaryFormatter(); // створення об'єкта для форматування
                BF.Serialize(S, D);
                S.Flush(); // очищення буфера потоку
                S.Close(); // закриття потоку
                this.Modify = false; // Заборона повторного запису
            }
            catch
            {

                MessageBox.Show("Помилка роботи з файлом"); // Виведення на екран повідомлення "Помилка роботи з файлом"

            }
        }
        public void ReadFromFile(System.Windows.Forms.DataGridView DG) // зчитування з файлу результатів
        {

            try
            {
                if (!File.Exists(this.OpenFileName))
                {
                    MessageBox.Show("файлу немає"); // Виведення на екран повідомлення"файлу немає"



                    return;
                }
                Stream S; // створення потоку
                S = File.Open(this.OpenFileName, FileMode.Open); // зчитування даних з файлу



                Buffer D;
                object O; // буферна змінна для контролю формату
                BinaryFormatter BF = new BinaryFormatter(); // створення об'єкта для форматування



                // формуємо таблицю
                System.Data.DataTable MT = new System.Data.DataTable();
                System.Data.DataColumn cKey = new System.Data.DataColumn("Ключ"); //формуємо колонку "Ключ"



                System.Data.DataColumn cInput = new

System.Data.DataColumn("Вхідні дані"); // формуємо колонку "Вхідні дані"

                System.Data.DataColumn cResult = new System.Data.DataColumn("Результат");

                // формуємо колонку "Результат"

                MT.Columns.Add(cKey); // додавання ключа
                MT.Columns.Add(cInput); // додавання вхідних даних
                MT.Columns.Add(cResult); // додавання результату
                while (S.Position < S.Length)
                {
                    O = BF.Deserialize(S); // десеріалізація
                    D = O as Buffer;
                    if (D == null) break;
                    System.Data.DataRow MR;
                    MR = MT.NewRow();
                    MR["Ключ"] = D.Key; // Занесення в таблицю номера
                    MR["Вхідні дані"] = D.Data; // Занесення в таблицю вхідних даних
                    MR["Результат"] = D.Result; // Занесення в таблицю результату
                    MT.Rows.Add(MR);
                }
                DG.DataSource = MT;
                S.Close(); // закриття
            }
            catch
            {
                MessageBox.Show("Помилка файлу"); // Виведення на екран повідомлення "файлу немає"

                
            }
        } // ReadFromFile закінчився
        public void Generator() // метод формування ключового поля
        {
            try
            {
                if (!File.Exists(this.SaveFileName)) // існує файл?
                {
                    Key = 1;
                    return;
                }
                Stream S; // створення потоку
                S = File.Open(this.SaveFileName, FileMode.Open); // Відкриття файлу 
                Buffer D;
                object O; // буферна змінна для контролю формату
                BinaryFormatter BF = new BinaryFormatter(); // створення елементу для форматування
                while (S.Position < S.Length)
                {
                    O = BF.Deserialize(S);
                    D = O as Buffer;
                    if (D == null) break;
                    Key = D.Key;
                }
                Key++;
                S.Close();
            }
            catch
            {
                MessageBox.Show("Помилка файлу"); // Виведення на екран повідомлення "Помилка файлу"
            }
        }
        public bool SaveFileNameExists()
        {
            if (this.SaveFileName == null)
                return false;
            else return true;
        }
        public void NewRec() // новий запис
        {
            this.Data = ""; // "" - ознака порожнього рядка
            this.Result = null; // для string- null
            Data = string.Empty; 
            Result = string.Empty;
            Modify = false;
            Key = 0;
        }
        public void Find(string Num) // пошук
        {
            int N;
            try
            {
                N = Convert.ToInt16(Num); // перетворення номера рядка в int16 для  відображення
            }
            catch
            {
                MessageBox.Show("помилка пошукового запиту"); // Виведення на  екран повідомлення "помилка пошукового запиту"



                return;
            }

            try
            {
                if (!File.Exists(this.OpenFileName))
                {
                    MessageBox.Show("файлу немає"); // Виведення на екран повідомлення "файлу немає"



                    return;
                }
                Stream S; // створення потоку
                S = File.Open(this.OpenFileName, FileMode.Open); // відкриття файлу
                Buffer D;
                object O; // буферна змінна для контролю формату
                BinaryFormatter BF = new BinaryFormatter(); // створення об'єкта для форматування



                while (S.Position < S.Length)
                {
                    O = BF.Deserialize(S);
                    D = O as Buffer;
                    if (D == null) break;
                    if (D.Key == N) // перевірка дорівнює чи номер пошуку номеру рядка в  таблиці
                    {
                        string ST;
                        ST = "Запис містить:" + (char)13 + "No" + Num + "Вхідні дані:" +

                        D.Data + "Результат:" + D.Result;

                        MessageBox.Show(ST, "Запис знайдена"); // Виведення на екран повідомлення "запис містить", номер, вхідних даних і результат
                        S.Close();
                        return;
                    }
                }
                S.Close();
                MessageBox.Show("Запис не знайдена"); // Виведення на екран повідомлення  "Запис не знайдена"
            }
            catch
            {
                MessageBox.Show("Помилка файлу"); // Виведення на екран повідомлення  "Помилка файлу"


            }
        } // Find закінчився
        public bool SaveTextFileNameExists()
        {
            if (this.SaveTextFileName == null)
                return false;
            else return true;
        }
        public string ReadSaveTextFileName()
        {
            return SaveTextFileName;
        }
        public void SaveToTextFile(string name, System.Windows.Forms.DataGridView D)
        {
            try
            {
                System.IO.StreamWriter textFile;
                if (!File.Exists(name))
                {
                    textFile = new System.IO.StreamWriter(name);
                }
                else
                {
                    textFile = new System.IO.StreamWriter(name, true);
                }
                for (int i = 0; i < D.RowCount - 1; i++)
                {
                    textFile.WriteLine("{0};{1};{2}", D[0, i].Value.ToString(), D[1,

                    i].Value.ToString(), D[2, i].Value.ToString());

                }
                textFile.Close();
            }
            catch
            {
                MessageBox.Show("Помилка роботи з файлом ");
            }
        }
    }
}
