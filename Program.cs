using System.Data;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using LinqToDB;
using Telegram.Bot.Polling;

namespace CalorieCounterBot
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Запускаю...");

            Guid AddProduct= Guid.Empty;
            var mode = AppMode.Default;

            string token = "5653252020:AAHUG-9o-oS46ygl4kEpnnfQwFsefoeTVjk";

            var client = new TelegramBotClient(token);

            var me = await client.GetMeAsync();

            var keyboardButtons1 = new KeyboardButton[1];
            keyboardButtons1[0] = new KeyboardButton("Добавить продукт");
            var keyboardButtons2 = new KeyboardButton[1];
            keyboardButtons2[0] = new KeyboardButton("Удалить продукт");
            var keyboardButtons3 = new KeyboardButton[1];
            keyboardButtons3[0] = new KeyboardButton("Добавить продукт в базу");
            var keyboardButtons4 = new KeyboardButton[1];
            keyboardButtons4[0] = new KeyboardButton("Изменить калорийность продукта");
            var keyboardButtons5 = new KeyboardButton[1];
            keyboardButtons5[0] = new KeyboardButton("Статистика");

            var rkm = new ReplyKeyboardMarkup(new[] { keyboardButtons1, keyboardButtons2, keyboardButtons3, keyboardButtons4, keyboardButtons5 });

            string MainMenu = @"
- /AddProducts - Добавить продукт
- /DeleteProducts - Удалить продукт
- /AddProductsToBase - Добавить продукт в базу
- /ChangeProductCalorie - Изменить калорийность продукта
- /Statistic - Статистика"";";

            client.StartReceiving(UpdateHandler, HandleErrorAsync);

            Console.WriteLine("Работаем!");

            static async Task HandleErrorAsync(ITelegramBotClient client, Exception exception, CancellationToken ct)
            {
                Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(exception));
            }
            
            async Task UpdateHandler(ITelegramBotClient client, Update update, CancellationToken ct)
            {
                if (update.Type == Telegram.Bot.Types.Enums.UpdateType.MyChatMember)
                {
                    return;
                }

                if (update.Message != null)
                {
                     Console.WriteLine($"Пришло сообщение от {update.Message.Chat.Id}  {update.Message.Chat.FirstName} {update.Message.Chat.LastName} : {update.Message.Text}  дата:  {update.Message.Date} ");
                }

                switch (mode)
                {
                    case AppMode.Default:
                        await DefaultHandler(client, update, ct);
                        break;

                    case AppMode.DeleteProducts:
                        await DeleteProductsHandler(client, update, ct);
                        break;

                    case AppMode.ChangeProductCalorie:
                        await ChangeProductCalorieHandler(client, update, ct);
                        break;

                    case AppMode.AddProducts:
                        await AddProductsHandler(client, update, ct);
                        break;

                    case AppMode.AddProductsToBase:
                        await AddProductsToBaseHandler(client, update, ct);
                        break;

                    case AppMode.NewCustomer:
                        await NewCustomerHandler(client, update, ct);
                        break;

                    case AppMode.Statistic:
                        await StatisticHandler(client, update, ct);
                        break;
                }

            }

#region DefaultHandler

            async Task DefaultHandler(
            ITelegramBotClient client,
            Update update,
            CancellationToken ct)
            {
                var text = update.Message.Text;

                var keyboardButtons11 = new KeyboardButton[1];
                keyboardButtons11[0] = new KeyboardButton("назад");
                var rkm1 = new ReplyKeyboardMarkup(new[] { keyboardButtons11});

                var keyboardButtons21 = new KeyboardButton[1];
                keyboardButtons21[0] = new KeyboardButton("День");
                var keyboardButtons22 = new KeyboardButton[1];
                keyboardButtons22[0] = new KeyboardButton("Неделя");
                var keyboardButtons23 = new KeyboardButton[1];
                keyboardButtons23[0] = new KeyboardButton("Месяц");
                var keyboardButtons24 = new KeyboardButton[1];
                keyboardButtons24[0] = new KeyboardButton("Год");
                var rkm2 = new ReplyKeyboardMarkup(new[] { keyboardButtons21, keyboardButtons22, keyboardButtons23, keyboardButtons24, keyboardButtons11 });

                switch (text)
                {
                    case "/start":
                        await client.SendTextMessageAsync(update.Message.Chat.Id, GetGreeting(update.Message.Chat));
                        break;

                    case "/help":
                        await client.SendTextMessageAsync(update.Message.Chat.Id, GetGreeting(update.Message.Chat));
                        break;

                    case "/AddProducts":
                        mode = AppMode.AddProducts;
                        await client.SendTextMessageAsync(update.Message.Chat.Id, DayCalories(update.Message.Chat.Id, 1, DateTime.Now.Date), replyMarkup: rkm1);
                        await client.SendTextMessageAsync(update.Message.Chat.Id, "Введите часть названия продукта от 3-х символов", replyMarkup: rkm1);
                        break;

                    case "Добавить продукт":
                        mode = AppMode.AddProducts;
                        await client.SendTextMessageAsync(update.Message.Chat.Id, DayCalories(update.Message.Chat.Id, 1, DateTime.Now.Date), replyMarkup: rkm1);
                        await client.SendTextMessageAsync(update.Message.Chat.Id, "Введите часть названия продукта от 3-х символов", replyMarkup: rkm1);
                        break;

                    case "/DeleteProducts":
                        mode = AppMode.DeleteProducts;
                        await client.SendTextMessageAsync(update.Message.Chat.Id, DayCalories(update.Message.Chat.Id, 0, DateTime.Now.Date), replyMarkup: rkm1);
                        break;

                    case "Удалить продукт":
                        mode = AppMode.DeleteProducts;
                        await client.SendTextMessageAsync(update.Message.Chat.Id, DayCalories(update.Message.Chat.Id, 0, DateTime.Now.Date), replyMarkup: rkm1); 
                        break;

                    case "/AddProductsToBase":
                        mode = AppMode.AddProductsToBase;
                        await client.SendTextMessageAsync(update.Message.Chat.Id, "Введите название продукта от 3-х символов и количество ккал в 100гр через запятую(например: Лосось,68)", replyMarkup: rkm1);
                        break;

                    case "Добавить продукт в базу":
                        mode = AppMode.AddProductsToBase;
                        await client.SendTextMessageAsync(update.Message.Chat.Id, "Введите название продукта от 3-х символов и количество ккал в 100гр через запятую(например: Лосось,68)", replyMarkup: rkm1);
                        break;

                    case "/ChangeProductCalorie":
                        mode = AppMode.ChangeProductCalorie;
                        await client.SendTextMessageAsync(update.Message.Chat.Id, "Введите название продукта от 3-х символов", replyMarkup: rkm1);
                        break;

                    case "Изменить калорийность продукта":
                        mode = AppMode.ChangeProductCalorie;
                        await client.SendTextMessageAsync(update.Message.Chat.Id, "Введите название продукта от 3-х символов", replyMarkup: rkm1);
                        break;

                    case "/Statistic":
                        mode = AppMode.Statistic;
                        await client.SendTextMessageAsync(update.Message.Chat.Id, $@"Введите: 0 - статистика за сегодняшний день
1 - статистика за неделю
2 - статистика за месяц
3 - статистика за год
День в формате : 23/12/2022 - для вывода статистики за этот день
Период формата : 23/12/2022-30/12/2022 - для вывода статистики за период
Период формата : 23/12/2022-12 - для вывода статистики за период, где 12 - количество дней в периоде", replyMarkup: rkm2);
                        break;

                    case "Статистика":
                        mode = AppMode.Statistic;
                        await client.SendTextMessageAsync(update.Message.Chat.Id, $@"Введите: 0 - статистика за сегодняшний день
1 - статистика за неделю
2 - статистика за месяц
3 - статистика за год
День в формате : 23/12/2022 - для вывода статистики за этот день
Период формата : 23/12/2022-30/12/2022 - для вывода статистики за период
Период формата : 23/12/2022-12 - для вывода статистики за период, где 12 - количество дней в периоде", replyMarkup: rkm2);
                        break;

                    default:
                        await client.SendTextMessageAsync(update.Message.Chat.Id, "Команда не распознана, попробуйте еще раз", replyMarkup: rkm);
                        break;
                }
            }
            #endregion


#region GetGreeting

            string GetGreeting(Chat chat)
            {

                Console.WriteLine(chat.FirstName + " " + chat.LastName);
                int Customer;
                using (ApplicationContext db = new ApplicationContext())
                {
                    Customer = db.Users.Where(x => x.User_ID == chat.Id).ToList().Count;
                }
                if (Customer > 0)
                {
                    return $@"Привет, {chat.FirstName} {chat.LastName}!
Меня зовут {me.Username}, вот варианты возможных действий:
                        
{MainMenu}";

                }
                else
                {
                    mode = AppMode.NewCustomer;
                    return $@"Привет, {chat.FirstName} {chat.LastName}!
Меня зовут {me.Username}, к сожалению я не нашёл Вас в нашей базе, поэтому давайте познакомимся.
Введите Ваш пол и год рождения через запятую:(например: М,2001 или Ж,1998)";
                }

            }
#endregion

#region NewCustomerHandler

            async Task NewCustomerHandler(
                ITelegramBotClient client,
                Update update,
                CancellationToken ct)
            {
                var text = update.Message?.Text?.Trim();
                if (!string.IsNullOrEmpty(text))
                {


                    string[] words = text.Split(new char[] { ',' });
                    if (words.Count() == 2 && int.TryParse(words[1], out int number) && (words[0]== "М" || words[0] == "Ж" || words[0] == "м" || words[0] == "ж") && number>1900 && number<2022)
                    {
                        Console.WriteLine($" 1 - {words[0]}  2-- {words[1]} ");

                        int gender;
                        if (words[0] == "М" || words[0] == "м") gender = 0;  else  gender = 1;
                        using (ApplicationContext db = new ApplicationContext())
                        {
                            Users user1 = new Users { Name = update.Message.Chat.FirstName + update.Message.Chat.LastName, User_ID = (long)update.Message.Chat.Id, Gender = gender, Birthday= number};
                            db.Users.Add(user1);
                            db.SaveChanges();
                        }
                            await client.SendTextMessageAsync(update.Message.Chat.Id,
                                $"Ваши данные успешно добавлены. Введите /help для справки.", replyMarkup: rkm);

                        mode = AppMode.Default;
                    }

                    else await client.SendTextMessageAsync(update.Message.Chat.Id, "Некорректный ввод, повторите");
                }
                else await client.SendTextMessageAsync(update.Message.Chat.Id, "Некорректный ввод, повторите");
            }
#endregion


#region AddProductsHandler

            async Task AddProductsHandler(
                ITelegramBotClient client,
                Update update,
                CancellationToken ct)
            {
                var text = update.Message?.Text?.Trim();

                if (text == "/exit" || text == "назад")
                {
                    AddProduct = Guid.Empty;
                    mode = AppMode.Default;
                    await client.SendTextMessageAsync(update.Message.Chat.Id, $@"Основное меню {MainMenu}", replyMarkup: rkm);
                }
                else if (!string.IsNullOrEmpty(text))
                {
                    List<Products> products=null;
                    if (AddProduct == Guid.Empty)
                    {
                        if (text.Count() >= 3)
                        {
                            string[] words = text.Split(new char[] { '@' });
                            var rows = new List<KeyboardButton[]>();
                            using (ApplicationContext db = new ApplicationContext())
                            {
                                var product = db.Products.Where(x => x.ProductName == words[0]).ToList();
                                if (product.Count == 0)
                                {
                                    AddProduct = Guid.Empty;
                                    products = db.Products.Where(x => x.ProductName.Contains(text)).ToList();
                                    foreach (Products u in products)
                                    {
                                        var kkal = db.ProductCalories.Where(x => x.ProductID == u.ProductID && x.UserID == update.Message.Chat.Id).FirstOrDefault();
                                        if (kkal == null) kkal = db.ProductCalories.Where(x => x.ProductID == u.ProductID).FirstOrDefault();
                                        var keyboardButtons1 = new KeyboardButton[1];
                                        keyboardButtons1[0] = new KeyboardButton(u.ProductName + "@(" +kkal.Calories+"ккал)");
                                        rows.Add(keyboardButtons1);
                                    }
                                }
                                else AddProduct = product.FirstOrDefault().ProductID;
                            }
                            if (AddProduct != Guid.Empty)
                            {
                                await client.SendTextMessageAsync(update.Message.Chat.Id, "Введите количество продукта в граммах или /exit");
                            }
                            else
                            {
                                var rkm = new ReplyKeyboardMarkup(rows.ToArray());
                                if (products?.Count == 0)
                                {
                                    await client.SendTextMessageAsync(update.Message.Chat.Id, "Продукт не найден, для поиска достаточно ввести часть названия продукта от 3-х символов или /exit", replyMarkup: rkm);
                                }
                                else await client.SendTextMessageAsync(update.Message.Chat.Id, "Выберите продукт или введите часть названия продукта от 3-х символов или /exit", replyMarkup: rkm);
                            }
                        }
                        else await client.SendTextMessageAsync(update.Message.Chat.Id, "Введите часть названия продукта от 3-х символов или /exit");
                    }
                    else
                    {
                        if (int.TryParse(text, out int number) && number > 0 && number <= 100000)
                        {
                            using (ApplicationContext db = new ApplicationContext())
                            {
                                UserProduct userProduct = new UserProduct {UserID = update.Message.Chat.Id, ProductID = AddProduct, Weight = number,  Date = DateTime.Now };
                                db.UserProduct.Add(userProduct);
                                db.SaveChanges();
                            }
                            await client.SendTextMessageAsync(update.Message.Chat.Id, "Продукт добавлен");
                            await client.SendTextMessageAsync(update.Message.Chat.Id, DayCalories(update.Message.Chat.Id, 1, DateTime.Now.Date));
                            AddProduct = Guid.Empty;
                            await client.SendTextMessageAsync(update.Message.Chat.Id, "Введите часть названия следующего продукта от 3-х символов или /exit");
                        }
                        else await client.SendTextMessageAsync(update.Message.Chat.Id, "Некорректный ввод, введите число от 1 до 100000 или /exit");
                    }
                }

            }
            #endregion


#region DeleteProductsHandler

            async Task DeleteProductsHandler(
                ITelegramBotClient client,
                Update update,
                CancellationToken ct)
            {
                var text = update.Message?.Text?.Trim();
                List<UserProduct> products;
                var i = 1; decimal count = 0;



                if (text == "/exit" || text == "назад")
                {
                    mode = AppMode.Default;
                    await client.SendTextMessageAsync(update.Message.Chat.Id, $@"Основное меню {MainMenu}", replyMarkup: rkm);
                }
                else if (!string.IsNullOrEmpty(text))
                {

                    using (ApplicationContext db = new ApplicationContext())
                    {
                        products = db.UserProduct.Where(x => x.Date >= DateTime.Now.Date && x.Date < DateTime.Now.Date.AddDays(1)).OrderByDescending(x => x.Date).ToList();
                    }
                    if (int.TryParse(text, out int number) && number >= 1 && number <= products.Count)
                    {
                        using (ApplicationContext db = new ApplicationContext())
                        {
                            db.UserProduct.Remove(products[number-1]);
                            db.SaveChanges();
                        }
                        await client.SendTextMessageAsync(update.Message.Chat.Id, $"Ваша запись успешно удалена");
                    }
                    //else
                    //  await client.SendTextMessageAsync(update.Message.Chat.Id, "Некорректный ввод, повторите или /exit");
                    await client.SendTextMessageAsync(update.Message.Chat.Id, DayCalories(update.Message.Chat.Id, 0, DateTime.Now.Date));
                }
            }
            #endregion


#region AddProductsToBaseHandler

            async Task AddProductsToBaseHandler(
                ITelegramBotClient client,
                Update update,
                CancellationToken ct)
            {
                var text = update.Message?.Text?.Trim();

                if (text == "/exit" || text == "назад")
                {
                    mode = AppMode.Default;
                    await client.SendTextMessageAsync(update.Message.Chat.Id, $@"Основное меню {MainMenu}", replyMarkup: rkm);
                }
                else if (!string.IsNullOrEmpty(text))
                {
                    string[] words = text.Split(new char[] { ',' });
                    if (words.Count()==2 && int.TryParse(words[1], out int number) && number > 0 && number < 10000)
                    {
                        using (ApplicationContext db = new ApplicationContext())
                        {
                            if (db.Products.Where(x => x.ProductName == words[0]).ToList().Count() == 0)
                            {
                                Products products1 = new Products { ProductName = words[0], ProductID = Guid.NewGuid() };
                                ProductCalories calories1 = new ProductCalories { ProductID = products1.ProductID, Calories = number };
                                db.Products.Add(products1);
                                db.SaveChanges();
                                db.ProductCalories.Add(calories1);
                                db.SaveChanges();
                                await client.SendTextMessageAsync(update.Message.Chat.Id, $"Ваши данные успешно добавлены");
                                await client.SendTextMessageAsync(update.Message.Chat.Id, "Введите название следующего продукта от 3-х символов и количество ккал в 100гр через запятую(например: Лосось,68) или /exit", replyMarkup: null);
                            }
                            else await client.SendTextMessageAsync(update.Message.Chat.Id, "Такой продукт уже есть в базе, введите название продукта от 3-х символов и количество ккал в 100гр через запятую(например: Лосось,68) или /exit", replyMarkup: null);
                        }
                    }
                    else await client.SendTextMessageAsync(update.Message.Chat.Id, "Некоректный ввод, введите название продукта от 3-х символов и количество ккал в 100гр через запятую(например: Лосось,68) или /exit", replyMarkup: null);

                }

            }
            #endregion


#region ChangeProductCalorieHandler

            async Task ChangeProductCalorieHandler(
                ITelegramBotClient client,
                Update update,
                CancellationToken ct)
            {
                var text = update.Message?.Text?.Trim();

                if (text == "/exit" || text == "назад")
                {
                    AddProduct = Guid.Empty;
                    mode = AppMode.Default;
                    await client.SendTextMessageAsync(update.Message.Chat.Id, $@"Основное меню {MainMenu}", replyMarkup: rkm);
                }
                else if (!string.IsNullOrEmpty(text))
                {
                    List<Products> products = null;
                    if (AddProduct == Guid.Empty)
                    {
                        if (text.Count() >= 3)
                        {
                            string[] words = text.Split(new char[] { '@' });
                            var rows = new List<KeyboardButton[]>();
                            using (ApplicationContext db = new ApplicationContext())
                            {
                                var product = db.Products.Where(x => x.ProductName == words[0]).ToList();
                                if (product.Count == 0)
                                {
                                    AddProduct = Guid.Empty;
                                    products = db.Products.Where(x => x.ProductName.Contains(text)).ToList();
                                    foreach (Products u in products)
                                    {
                                        var kkal = db.ProductCalories.Where(x => x.ProductID == u.ProductID && x.UserID == update.Message.Chat.Id).FirstOrDefault();
                                        if (kkal == null) kkal = db.ProductCalories.Where(x => x.ProductID == u.ProductID).FirstOrDefault();
                                        var keyboardButtons1 = new KeyboardButton[1];
                                        keyboardButtons1[0] = new KeyboardButton(u.ProductName + "@(" + kkal.Calories + "ккал)");
                                        rows.Add(keyboardButtons1);
                                    }
                                }
                                else AddProduct = product.FirstOrDefault().ProductID;
                            }
                            if (AddProduct != Guid.Empty)
                            {
                                await client.SendTextMessageAsync(update.Message.Chat.Id, "Введите калорийность продукта в ккал или /exit");
                            }
                            else
                            {
                                var rkm = new ReplyKeyboardMarkup(rows.ToArray());
                                if (products?.Count == 0)
                                {
                                    await client.SendTextMessageAsync(update.Message.Chat.Id, "Продукт не найден, для поиска достаточно ввести часть названия продукта от 3-х символов или /exit", replyMarkup: rkm);
                                }
                                else await client.SendTextMessageAsync(update.Message.Chat.Id, "Выберите продукт или введите часть названия продукта от 3-х символов или /exit", replyMarkup: rkm);
                            }
                        }
                        else await client.SendTextMessageAsync(update.Message.Chat.Id, "Введите часть названия продукта от 3-х символов или /exit");
                    }
                    else
                    {
                        if (int.TryParse(text, out int number) && number > 0 && number <= 100000)
                        {
                            using (ApplicationContext db = new ApplicationContext())
                            {
                                var kkal=db.ProductCalories.Where(x => x.ProductID == AddProduct && x.UserID == update.Message.Chat.Id).FirstOrDefault();
                                if (kkal == null)
                                {
                                    ProductCalories ProductCalories1 = new ProductCalories { UserID = update.Message.Chat.Id, ProductID = AddProduct, Calories = number};
                                    db.ProductCalories.Add(ProductCalories1);
                                    db.SaveChanges();
                                }
                                else
                                {
                                    kkal.Calories = number;
                                    db.SaveChanges();
                                }
                            }
                            await client.SendTextMessageAsync(update.Message.Chat.Id, "Калорийность продукта изменена");
                            AddProduct = Guid.Empty;
                            await client.SendTextMessageAsync(update.Message.Chat.Id, "Введите часть названия следующего продукта от 3-х символов или /exit");
                        }
                        else await client.SendTextMessageAsync(update.Message.Chat.Id, "Некорректный ввод, введите число от 1 до 100000 или /exit");
                    }
                }
            }
            #endregion


#region StatisticHandler

            async Task StatisticHandler(
                ITelegramBotClient client,
                Update update,
                CancellationToken ct)
            {
                var text = update.Message?.Text?.Trim();
                List<UserProduct> products;
                var i = 1; decimal count = 0;



                if (text == "/exit" || text == "назад")
                {
                    mode = AppMode.Default;
                    await client.SendTextMessageAsync(update.Message.Chat.Id, $@"Основное меню {MainMenu}", replyMarkup: rkm);
                }
                else if (!string.IsNullOrEmpty(text))
                {
                    string[] words = text.Split(new char[] { '-' });
                    if (text == "0" || text == "День") await client.SendTextMessageAsync(update.Message.Chat.Id, DayCalories(update.Message.Chat.Id, 2, DateTime.Now.Date));
                    else if (text == "1" || text == "Неделя") await client.SendTextMessageAsync(update.Message.Chat.Id, DayCalories(update.Message.Chat.Id, 2, DateTime.Now.Date.AddDays(-6),7));
                    else if (text == "2" || text == "Месяц") await client.SendTextMessageAsync(update.Message.Chat.Id, DayCalories(update.Message.Chat.Id, 3, DateTime.Now.Date.AddDays(-29), 30));
                    else if (text == "3" || text == "Год") await client.SendTextMessageAsync(update.Message.Chat.Id, DayCalories(update.Message.Chat.Id, 3, DateTime.Now.Date.AddDays(-364), 365));
                    else if (words.Count() == 1 && DateTime.TryParse(text, out DateTime date)) await client.SendTextMessageAsync(update.Message.Chat.Id, DayCalories(update.Message.Chat.Id, 2, date));
                    else if (words.Count() == 2 && DateTime.TryParse(words[0], out DateTime date1) && DateTime.TryParse(words[1], out DateTime date2))
                    {
                        double diff = date2.Subtract(date1).TotalDays;
                        if (diff > 7) { await client.SendTextMessageAsync(update.Message.Chat.Id, DayCalories(update.Message.Chat.Id, 3, date1,(int)diff+1)); }
                        else await client.SendTextMessageAsync(update.Message.Chat.Id, DayCalories(update.Message.Chat.Id, 2, date1, (int)diff+1));
                    }
                    else if (words.Count() == 2 && DateTime.TryParse(words[0], out DateTime date3) && int.TryParse(words[1], out int number)) await client.SendTextMessageAsync(update.Message.Chat.Id, DayCalories(update.Message.Chat.Id, 2, date3, number));
                    else await client.SendTextMessageAsync(update.Message.Chat.Id, "Некорректный ввод, попробуйте еще раз или /exit");
                }
            }
#endregion

            Console.ReadKey();
        }



        public static string DayCalories(long Id, int type, DateTime dateTime, int period = 1)
        {
            List<UserProduct> products;string addition=""; string addition1 = "";
            var i = 1; decimal count = 0;string prodList = ""; string ret = "";
            decimal countAll = 0; int days = 0;
            for (var j = 0; j < period; j++)
            {
                i = 1; prodList = ""; count = 0;
                using (ApplicationContext db = new ApplicationContext())
                {
                    products = db.UserProduct.Where(x => x.Date >= dateTime && x.Date < dateTime.AddDays(1)).OrderByDescending(x => x.Date).ToList();
                    foreach (UserProduct u in products)
                    {
                        var prod = db.Products.Where(x => x.ProductID == u.ProductID).FirstOrDefault();
                        var kkal = db.ProductCalories.Where(x => x.ProductID == u.ProductID && x.UserID == Id).FirstOrDefault();
                        if (kkal == null) kkal = db.ProductCalories.Where(x => x.ProductID == u.ProductID).FirstOrDefault();
                        count += kkal.Calories * u.Weight / 100;
                        if (type != 3) prodList += $@"{i}. {prod.ProductName} - {kkal.Calories * u.Weight / 100}ккал ({u.Weight}гр. по {kkal.Calories}ккал)
";
                        i++;
                    }
                }
                if (type == 0) addition = $@"
Введите номер продукта(от 1 до {products.Count}), который вы хотите удалить или /exit";
                if (type == 2 || type == 3) addition1 = $@"
{dateTime.ToShortDateString()}
";

                if ((type == 0 || type == 1) && i == 1) ret = "За день еще не добавлено продуктов";
                else if (i != 1)
                {
                    ret += $@"{addition1}
За день заведено {products.Count} продуктов
{prodList}Калорийность продуктов за день: {count}
{addition}";
                    days++;
                }
                dateTime = dateTime.AddDays(1);
                countAll = countAll + count;
            }
            if(ret == "" && (type == 2 || type == 3)) ret = "За период не добавлено продуктов";
            else if (ret != "" && (type == 2 || type == 3)) ret += $@"
Общая калорийность продуктов: {countAll}
Средняя калорийность продуктов за день: {Math.Round(countAll/ days,2)}";
            return ret;
        }

    }
        
    
    enum AppMode
    {
        Default = 0,
        AddProducts = 1,
        DeleteProducts = 2,
        NewCustomer = 3,
        AddProductsToBase = 4,
        ChangeProductCalorie = 5,
        Statistic = 6,
    }
}