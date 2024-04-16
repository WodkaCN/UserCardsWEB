using UserCardsAPI.Models.DB.Entity;

namespace UserCardsAPI.Models.DB.EntityEx
{
    public static class DTOCardsInfo
    {
        public static IQueryable<CardsInfo> GetCardInfo(DBUserCardsContext context, String? UID, Int64? ID = null)
        {
            var data = from t in context.CardsInfos
                       where
                       (UID == null || UID == t.Uid)
                       &&
                       (ID == null || ID == t.Id)
                       select t;

            return data;
        }

        public static List<CardsInfo> GetCardsInfoList(String? UID = null, Int64? ID = null)
        {
            using (var context = new DBUserCardsContext())
            {
                var data = GetCardInfo(context, UID, ID).OrderByDescending(t => t.Uid).ToList();

                return data;
            }
        }

        public static Int64 AddCardsInfo(CardsInfo entity)
        {
            try
            {
                using (var context = new DBUserCardsContext())
                {
                    context.CardsInfos.Add(entity);
                    context.SaveChanges();
                    return (long)entity.Id;
                }
            }
            catch
            {
                throw;
            }
        }

        public static Int64 UpdateCardsInfo(CardsInfo entity)
        {
            try
            {
                using (var context = new DBUserCardsContext())
                {
                    context.Update<CardsInfo>(entity);
                    context.SaveChanges();
                    return (long)entity.Id;
                }
            }
            catch
            {
                throw;
            }
        }

        public static void DeleteCardsInfo(Int64 ID)
        {
            try
            {
                using (var context = new DBUserCardsContext())
                {
                    var entity = context.CardsInfos.FirstOrDefault(e => ID == e.Id);

                    if (entity != null)
                    {
                        context.Remove<CardsInfo>(entity);
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
