using NickBuhro.Translit;
using UserCardsDB_DeployTool.Models.DB.Entity;

namespace UserCardsDB_DeployTool.Managers
{
    public class FakeGenerator
    {
        private string[] _fakeF =
        {
            "Яковлев", "Константинов", "Рогов", "Романов", "Субботин",
            "Панкратов", "Соколов", "Андрианов", "Петров", "Андреев",
            "Степанов", "Давыдов", "Казаков", "Латышев", "Логинов"
        };

        private string[] _fakeI =
        {
            "Роман", "Максим", "Егор", "Дмитрий", "Фёдор",
            "Александр", "Лев", "Никита", "Глеб", "Леонид",
            "Николай", "Тимур", "Тимофей", "Владислав", "Ян"
        };

        private string[] _fakeO =
        {
            "Макарович", "Эминович", "Миронович", "Александрович", "Васильевич",
            "Глебович", "Вадимович", "Русланович", "Иванович", "Кириллович",
            "Евгеньевич", "Богданович", "Ибрагимович", "Дмитриевич", "Львович"
        };

        private string[] _fakeWorkPlace =
        {
            "СБЕР", "Тинькофф", "Альфа-Банк", "Центральный Банк РФ"
        };

        public List<FakesPair> GenerateFakeData(int fakeNum)
        {
            var rnd = new Random();
            var fakesList = new List<FakesPair>();

            for (int i = 0; i < fakeNum; i++)
            {
                var ciList = new List<CardInfo>();

                var guid = Guid.NewGuid().ToString().ToUpper();
                var fakeCardHolder = new CardHolder()
                {
                    UID = guid,
                    F = _fakeF[rnd.Next(15)],
                    I = _fakeI[rnd.Next(15)],
                    O = _fakeO[rnd.Next(15)],
                    BirthDate = new DateTime(rnd.Next(1950, 2007), rnd.Next(1, 13), rnd.Next(1, 29)).Date,
                    WorkPlace = _fakeWorkPlace[rnd.Next(4)]
                };
                var cardsCnt = rnd.Next(1, 4);

                for (int j = 0; j < cardsCnt; j++)
                {
                    var fakeCardInfo = new CardInfo()
                    {
                        UID = guid,
                        FIOLat = Transliteration.CyrillicToLatin($"{fakeCardHolder.F} {fakeCardHolder.I} {fakeCardHolder.O}"),
                        CardNumber = GenerateFakeCardNumber(rnd),
                        CardAccount = GenerateFakeAccount(rnd),
                        Expire = new DateTime(rnd.Next(2024, 2035), rnd.Next(1, 13), rnd.Next(1, 29)).Date,
                        CVC = rnd.Next(1, 999).ToString().PadLeft(3, '0')
                };

                    ciList.Add(fakeCardInfo);
                }

                fakesList.Add(new FakesPair()
                {
                    CardHolder = fakeCardHolder,
                    CardsInfo = ciList
                });
            }

            return fakesList;
        }

        private string GenerateFakeCardNumber(Random rnd)
        {
            var cardPaymentSystem = rnd.Next(2, 7).ToString();
            var cardBankBIС = rnd.Next(1, 99999).ToString().PadLeft(5, '0');
            var cardAccount = rnd.Next(1, 999999999).ToString().PadLeft(9, '0');
            var cardControlNum = rnd.Next(0, 10).ToString();

            return $"{cardPaymentSystem}{cardBankBIС}{cardAccount}{cardControlNum}";
        }

        private string GenerateFakeAccount(Random rnd)
        {
            var nums45 = rnd.Next(1, 99).ToString().PadLeft(2, '0');
            var num9 = rnd.Next(0, 10).ToString();
            var nums10111213 = rnd.Next(1, 9999).ToString().PadLeft(4, '0');
            var uniqueID = rnd.Next(1, 9999999).ToString().PadLeft(7, '0');

            return $"408{nums45}810{num9}{nums10111213}{uniqueID}";
        }
    }
}
