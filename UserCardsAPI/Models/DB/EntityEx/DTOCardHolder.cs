using NickBuhro.Translit;
using UserCardsAPI.Models.DB.Entity;

namespace UserCardsAPI.Models.DB.EntityEx
{
    public static class DTOCardHolder
    {
        public static IQueryable<CardHolder> GetCardHolder(DBUserCardsContext context, String? UID = null)
        {
            var data = from t in context.CardHolders
                       where
                       (UID == null || UID == t.Uid)
                       select t;

            return data;
        }

        public static List<CardHolder> GetCardHoldersList(String? UID = null)
        {
            using (var context = new DBUserCardsContext())
            {
                var data = GetCardHolder(context, UID).OrderByDescending(t => t.Uid).ToList();

                return data;
            }
        }

        public static String AddCardHolder(CardHolder entity)
        {
            try
            {
                using (var context = new DBUserCardsContext())
                {
                    context.CardHolders.Add(entity);
                    context.SaveChanges();
                    return entity.Uid;
                }
            }
            catch
            {
                throw;
            }
        }

        public static String UpdateCardHolder(CardHolder entity)
        {
            try
            {
                using (var context = new DBUserCardsContext())
                {
                    context.Update<CardHolder>(entity);
                    context.SaveChanges();
                    return entity.Uid;
                }
            }
            catch
            {
                throw;
            }
        }

        public static void DeleteCardHolder(String UID)
        {
            try
            {
                using (var context = new DBUserCardsContext())
                {
                    var entity = context.CardHolders.FirstOrDefault(e => UID == e.Uid);

                    if (entity != null)
                    {
                        context.Remove<CardHolder>(entity);
                        context.SaveChanges();
                    }
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
