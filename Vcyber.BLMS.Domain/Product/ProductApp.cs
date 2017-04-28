using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.Common;
using Vcyber.BLMS.Domain.Common;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Entity.Enum;
using Vcyber.BLMS.Entity.SelectCondition;
using Vcyber.BLMS.IRepository;

namespace Vcyber.BLMS.Domain
{
    /// <summary>
    /// 商品业务
    /// </summary>
    public class ProductApp : IProductApp
    {
        #region ==== 构造函数 ====

        public ProductApp() { }

        #endregion

        #region ==== 前台管理 ====

        /// <summary>
        /// 分页获取商品信息
        /// </summary>
        /// <param name="pageData"></param>
        /// <param name="Total"></param>
        /// <returns></returns>
        public IEnumerable<Product> GetPageProduct(PageData pageData, out int Total)
        {
            return _DbSession.ProductStorager.GetPageProduct(pageData, out Total);
        }

        /// <summary>
        /// 分页获取商品信息
        /// </summary>
        /// <param name="categoryId">商品类型Id</param>
        /// <param name="pageData"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public IEnumerable<Product> GetPageProduct(int categoryId, PageData pageData, out int total)
        {
            return _DbSession.ProductStorager.GetPageProduct(categoryId, pageData, out total);
        }

        public Product GetProductById(int id)
        {
            return _DbSession.ProductStorager.GetProductById(id);
        }

        public int RecentlyProduct(DateTime dateTime)
        {
            return _DbSession.ProductStorager.RecentlyProduct(dateTime);
        }

        /// <summary>
        /// 获取商品信息
        /// </summary>
        /// <param name="type">商品类型</param>
        /// <param name="mode">商品形式</param>
        /// <returns></returns>
        public IEnumerable<Product> GetProduct(EProductType type, EProductMode mode)
        {
            return _DbSession.ProductStorager.GetProduct(type, mode);
        }

        /// <summary>
        /// 根据推荐商品类型获取商品信息
        /// </summary>
        /// <param name="recommend">推荐类型</param>
        /// <param name="pageData"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public IEnumerable<Product> GetProduct(EProductRecommend recommend, PageData pageData, out int total)
        {
            return _DbSession.ProductStorager.GetProduct(recommend, pageData, out total);
        }

        /// <summary>
        /// 获取销售排行商品
        /// </summary>
        /// <param name="pageData"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public IEnumerable<Product> GetSalesProduct(PageData pageData, out int total)
        {
            return _DbSession.ProductStorager.GetSalesProduct(pageData, out total);
        }

        /// <summary>
        /// 用户是否可以赞美图片或者商品
        /// </summary>
        /// <param name="type">赞美类型</param>
        /// <param name="memberId">会员id</param>
        /// <param name="value">赞美对象Id</param>
        /// <returns></returns>
        public bool IsParaise(EPraiseType type, string memberId, int value,out int count)
        {
            count = 0;

            switch (type)
            {
                case EPraiseType.Img:return _AppContext.BreadApp.IsEmiricBread(EEmpiricRule.网站活动点赞, memberId,out count);
                    break;
                case EPraiseType.Product:return _AppContext.BreadApp.IsEmiricBread(EEmpiricRule.商城礼品点赞, memberId,out count);
                    break;
                default:
                    break;
            }

            return false;
        }

        /// <summary>
        /// 商品和图片赞美次数
        /// </summary>
        /// <param name="type">赞美类型</param>
        /// <param name="value">赞美对象Id</param>
        /// <returns></returns>
        public int PraiseCount(EPraiseType type, int value)
        {
            switch (type)
            {
                case EPraiseType.Img: return _DbSession.ImgPraiseRecordStorager.FindCount(value);
                    break;
                case EPraiseType.Product: return _DbSession.ProductStorager.FindPraiseCount(value);
                    break;
                default:
                    break;
            }

            return 0;
        }

        /// <summary>
        /// 赞美商品或者图片
        /// </summary>
        /// <param name="type">赞美类型</param>
        /// <param name="memberId">会员Id</param>
        /// <param name="level">会员级别</param>
        /// <param name="value">赞美对象Id</param>
        /// <returns></returns>
        public bool Praise(EPraiseType type, string memberId, MemshipLevel level, int value)
        {

            switch (type)
            {
                case EPraiseType.Img:
                    {
                        int ovalue;
                        _DbSession.ImgPraiseRecordStorager.Add(new ImagePraiseRecord() {  ImgId=value, MemberId=memberId, CreateTime=DateTime.Now, Remark=""});
                        //_AppContext.BreadApp.BlueBeanBread(EBRuleType.网站活动点赞, memberId, level, out ovalue);
                        //_AppContext.BreadApp.EmpiricBread(EEmpiricRule.网站活动点赞, memberId, out ovalue);
                        return true;
                    }
                    break;
                case EPraiseType.Product:
                    {
                        bool result = _DbSession.ProductStorager.AddPraiseCount(value);

                        if (result)
                        {
                            //int ovalue;
                            //_AppContext.BreadApp.BlueBeanBread(EBRuleType.商城礼品点赞,memberId,level,out ovalue);
                            //_AppContext.BreadApp.EmpiricBread(EEmpiricRule.商城礼品点赞, memberId, out ovalue);
                            return true;
                        }
                    }
                    break;
                default:
                    break;
            }

            return false;
        }

        /// <summary>
        /// 赞美商品
        /// </summary>
        /// <param name="id">商品id</param>
        /// <returns></returns>
        public bool Praise(int id)
        {
            return _DbSession.ProductStorager.AddPraiseCount(id);
        }

        /// <summary>
        /// 获取商品列表
        /// </summary>
        /// <param name="condition">商品搜索条件</param>
        /// <param name="pageData"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public IEnumerable<Product> GetProduct(ProductSearchCondition condition, PageData pageData, out int total)
        {
            return _DbSession.ProductStorager.GetProduct(condition, pageData, out total);
        }

        /// <summary>
        /// 获取商品列表(生日特权专用)
        /// </summary>
        /// <param name="condition">商品搜索条件</param>
        /// <param name="pageData"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public IEnumerable<Product> GetProductByBirthday(ProductSearchCondition condition, PageData pageData, out int total)
        {
            return _DbSession.ProductStorager.GetProductByBirthday(condition, pageData, out total);
        }

        /// <summary>
        /// 判断某个用户在当年是否购买过某个类型的产品
        /// </summary>
        /// <param name="categoryId">类型ID</param>
        /// <param name="userID">用户ID</param>
        /// <returns>购买的条数</returns>
        public int checkProduct(int categoryId, string userID) 
        {
            return _DbSession.ProductStorager.checkProduct(categoryId, userID);
        }

        // <summary>
        /// 获取商品信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Product GetProduct(int id, EProductState productState, EDataStatus dataStatus)
        {
            var item = _DbSession.ProductStorager.GetProduct(id, productState, dataStatus);

            if (item != null)
            {
                item.Category = _DbSession.ProductStorager.GetProductCategory(item.ID);

                if (item.Category != null)
                {
                    var category = _DbSession.CategoryStorager.SelectOne(item.Category.CategoryID);

                    if (category.ParentID == 0)
                    {
                        item.ParentCategory = category;
                        item.ChildCategory = new Category();
                    }
                    else
                    {
                        item.ChildCategory = category;
                        item.ParentCategory = _DbSession.CategoryStorager.SelectOne(item.ChildCategory.ParentID);
                    }
                }

                var imgs = _DbSession.ProductStorager.GetImage(item.ID);
                item.Images = imgs != null && imgs.Count() > 0 ? imgs.ToList<ProductImage>() : new List<ProductImage>(6);
                item.Detail = _DbSession.ProductStorager.GetDetail(item.ID);
            }

            return item;
        }

        /// <summary>
        /// 发放卡券
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="activityType">活动类型</param>
        /// <param name="count">发放数量</param>
        /// <param name="source">来源</param>
        public void SendCard(string userId, string activityType,string cardName, int count, string source = "blms_wechat") 
        {
            try
            {
                //处理电子卡券                        
                var member = _AppContext.DealerMembershipApp.GetMembershipByUserId(userId);
                var cardCodes = "";
                var customCardQuery = _AppContext.CustomCardInfoApp.GetCustomCardInfoListByActType(activityType, cardName);
                //如果用户存在并且卡券类型存在
                if (member != null && customCardQuery.Count() > 0)
                {
                    var customCardObj = customCardQuery.First();
                    //根据发放数量发放
                    for (int i = 0; i < count; i++)
                    {
                        var cardCode = string.Empty;
                        //2为合作商品，其他为自有卡券
                        if (customCardObj.CardSource == 2)
                        {
                            //获取一个卡券
                            var merchant = _AppContext.CustomCardMerchantConsumeCodeApp.GetSingleCardMerchantConsumeCode(customCardObj.CardType);
                            //如果库存充足
                            if (merchant != null)
                            {
                                //卡券码                            
                                cardCode = string.Format("{0}[{1}]", customCardObj.CardType, merchant.CardCode);
                            }
                        }
                        else
                        {
                            cardCode = IdGenerator.GetId(SequenceCategory.BH);
                        }
                        log4net.LogManager.GetLogger("domain_productapp_sendcard_method").Info(string.Format("UserId:{0},券码为：{1}", userId, cardCode));
                        //如果取到卡券
                        if (!string.IsNullOrEmpty(cardCode))
                        {
                            cardCodes += (cardCode + ",");
                            //创建一条卡券领取记录
                            var customcard = new CustomCard()
                            {
                                CardType = customCardObj.CardType,
                                CardCode = cardCode,
                                CardId = customCardObj.Id,
                                CreateTime = DateTime.Now,
                                IsSave = true,
                                IsCancel = false,
                                UserId = userId,
                                IsReissue = false,
                                Tel = member.UserName,
                                IsSend = true,
                                OpenId = "",
                                Source = source
                            };
                            var addResult = _AppContext.CustomCardApp.AddCustomCard(customcard);
                            if (addResult.IsSuccess)
                            {
                                //减库存
                                _AppContext.CustomCardInfoApp.UpdateCustomCardQuantityByType(customcard.CardType);
                            }
                            else
                            {
                                log4net.LogManager.GetLogger("domain_productapp_sendcard_method").Info(string.Format("UserId:{0}，下发卡券失败:{1}", userId, addResult.Message));
                            }
                        }
                        else
                        {
                            log4net.LogManager.GetLogger("domain_productapp_sendcard_method").Info(string.Format("UserId:{0}的券码为空", userId));
                        }
                    }
                    //如果取到卡券码就发短信
                    if (!string.IsNullOrEmpty(cardCodes) && customCardObj.CardSource == 2)
                    {
                        var cardCodeString = cardCodes.TrimEnd(',');
                        if (!string.IsNullOrEmpty(customCardObj.SmsContent))
                        {
                            //发送卡券短信信息；
                            _AppContext.CustomCardApp.SendCustomCardSms(customCardObj, new CustomCardSms() { CardCode = cardCodeString }, member.UserName);
                        }
                        else
                        {
                            if (!cardName.Contains("途家"))
                            {
                                _AppContext.SMSApp.SendSMS(ESmsType.电子卡券, member.UserName, new string[] { "星巴克", cardCodeString }, false);                            
                            }
                        }
                    }
                }
                else
                {
                    log4net.LogManager.GetLogger("domain_productapp_sendcard_method").Info(string.Format("用户不存在或卡券不存在,UserId:{0},CardName:{1}", userId, cardName));
                }
            }
            catch (Exception ex)
            {
                log4net.LogManager.GetLogger("domain_productapp_sendcard_method").Error("下发卡券时出错：", ex);
            }
        }

        #endregion

        #region ==== 后台管理 ====

        /// <summary>
        /// 添加商品信息
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int AddProduct(Product entity)
        {
            //using (TransactionScope tran = new TransactionScope())
            //{
            int pid = _DbSession.ProductStorager.AddProduct(entity);

            if (entity.Images != null)
            {
                ProductImage img = null;

                foreach (ProductImage item in entity.Images)
                {
                    img = new ProductImage { ProductID = pid, Image = item.Image, IsDefault = item.IsDefault };
                    int imgid = _DbSession.ProductStorager.AddImage(img);

                    if (item.IsDefault == 1)//默认图片
                    {
                        _DbSession.ProductStorager.SetDefaultImage(pid, imgid);
                    }
                }
            }

            if (entity.Detail != null)
            {
                entity.Detail.ProductID = pid;
                _DbSession.ProductStorager.AddDetail(entity.Detail);
            }

            if (entity.Category != null)
            {
                entity.Category.ProductID = pid;
                _DbSession.ProductStorager.AddProductCategory(entity.Category);
            }

            //tran.Complete();
            return pid;
            //}
        }

        /// <summary>
        /// 编辑商品信息
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool EditProduct(Product entity)
        {
            //using (TransactionScope tran = new TransactionScope())
            //{
            bool bol = _DbSession.ProductStorager.EditProduct(entity);

            if (entity.Detail != null)
            {
                entity.Detail.ProductID = entity.ID;
                _DbSession.ProductStorager.EditDetail(entity.Detail);
            }

            if (entity.Images != null && entity.Images.Count() > 0)
            {
                foreach (ProductImage item in entity.Images)
                {
                    int imgid = item.ID;
                    item.ProductID = entity.ID;
                    item.Image = item.Image;

                    if (item.ID == 0 || _DbSession.ProductStorager.GetImgOne(item.ID) == null)
                    {
                        item.ProductID = entity.ID;
                        imgid = _DbSession.ProductStorager.AddImage(item);
                    }
                    else
                    {
                        _DbSession.ProductStorager.UpdateImg(item);
                    }

                    if (item.IsDefault == 1)//默认图片
                    {
                        _DbSession.ProductStorager.SetDefaultImage(entity.ID, imgid);
                    }
                }
            }

            if (entity.Category != null)
            {
                var categpry = _DbSession.ProductStorager.GetProductCategory(entity.ID);

                if (categpry != null && categpry.CategoryID != entity.Category.CategoryID)
                {
                    categpry.CategoryID = entity.Category.CategoryID;
                    _DbSession.ProductStorager.EditProductCategory(categpry);
                }

                if (categpry == null)
                {
                    categpry = new ProductCategory();
                    categpry.CategoryID = entity.Category.CategoryID;
                    categpry.ProductID = entity.ID;
                    _DbSession.ProductStorager.AddProductCategory(categpry);
                }
            }

            //tran.Complete();
            return bol;
            //}
        }

        /// <summary>
        /// 删除商品信息
        /// </summary>
        /// <param name="id">商品Id</param>
        /// <returns></returns>
        public bool DeleteProduct(int id)
        {
            var product = _DbSession.ProductStorager.GetProduct(id);

            if (product != null && product.LQty > 0)
            {
                throw new RepeatException("商品还有没有处理的订单。");
            }

            return _DbSession.ProductStorager.DeleteProduct(id);
        }

        /// <summary>
        /// 上架商品
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool PutOnProduct(int id)
        {
            return _DbSession.ProductStorager.PutOnProduct(id);
        }

        /// <summary>
        /// 下架商品
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool PutOffProduct(int id)
        {
            return _DbSession.ProductStorager.PutOffProduct(id);
        }

        /// <summary>
        /// 获取商品信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Product GetProduct(int id)
        {
            var item = _DbSession.ProductStorager.GetProduct(id);
            
            if (item != null)
            {
                item.Category = _DbSession.ProductStorager.GetProductCategory(item.ID);

                if (item.Category != null && item.Category.CategoryName == "生日特权")
                {
                    
                    var category = _DbSession.CategoryStorager.SelectBirthdayOne(item.Category.CategoryID);
                    if (category.ParentID == 0)
                    {
                        item.ParentCategory = category;
                        item.ChildCategory = new Category();
                    }
                    else
                    {
                        item.ChildCategory = category;
                        item.ParentCategory = _DbSession.CategoryStorager.SelectOne(item.ChildCategory.ParentID);
                    }

                }
                //if (item.Category != null)
                else
                {
                    var category = _DbSession.CategoryStorager.SelectOne(item.Category.CategoryID);
                    
                    if (category.ParentID == 0)
                    {
                        item.ParentCategory = category;
                        item.ChildCategory = new Category();
                    }
                    else
                    {
                        item.ChildCategory = category;
                        item.ParentCategory = _DbSession.CategoryStorager.SelectOne(item.ChildCategory.ParentID);
                    }
                }

                var imgs = _DbSession.ProductStorager.GetImage(item.ID);
                item.Images = imgs != null && imgs.Count() > 0 ? imgs.ToList<ProductImage>() : new List<ProductImage>(6);
                item.Detail = _DbSession.ProductStorager.GetDetail(item.ID);
            }

            return item;
        }

        /// <summary>
        /// 编辑商品库存
        /// </summary>
        /// <param name="id"></param>
        /// <param name="qty">商品库存量</param>
        /// <returns></returns>
        public bool EditQty(int id, int qty)
        {
            return _DbSession.ProductStorager.EditQty(id, qty);
        }

        /// <summary>
        /// 设置商品权重
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="weight">权重</param>
        /// <returns></returns>
        public bool SetWeight(int productId, int weight)
        {
            return _DbSession.ProductStorager.SetWeight(productId, weight);
        }

        /// <summary>
        /// 设置商品推荐
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="recommend"></param>
        /// <returns></returns>
        public bool SetRecommend(int productId, EProductRecommend recommend)
        {
            return _DbSession.ProductStorager.SetRecommend(productId, recommend);
        }

        /// <summary>
        /// 搜索商品信息列表
        /// </summary>
        /// <param name="query"></param>
        /// <param name="pageData"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public IEnumerable<Product> GetPageProduct(ProductCondition condition, PageData pageData, out int total)
        {
            var produts = _DbSession.ProductStorager.GetPageProduct(condition, pageData, out total);

            if (produts != null && produts.Count() > 0)
            {
                foreach (var item in produts)
                {
                    item.Category = _DbSession.ProductStorager.GetProductCategory(item.ID);
                    var imgs = _DbSession.ProductStorager.GetImage(item.ID);
                    item.Images = imgs != null && imgs.Count() > 0 ? imgs.ToList<ProductImage>() : new List<ProductImage>(1);
                    item.Detail = _DbSession.ProductStorager.GetDetail(item.ID);
                }
            }

            return produts;
        }

        /// <summary>
        /// 商品统计
        /// </summary>
        /// <returns></returns>
        public ProductStatistics Statistics()
        {
            return _DbSession.ProductStorager.Statistics();
        }

        /// <summary>
        /// 添加商品图片
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int AddImage(ProductImage entity)
        {
            return _DbSession.ProductStorager.AddImage(entity);
        }

        /// <summary>
        /// 删除商品图片
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteImage(int id)
        {
            return _DbSession.ProductStorager.DeleteImage(id);
        }

        /// <summary>
        /// 设置商品默认图片
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="imgid"></param>
        /// <returns></returns>
        public bool SetDefaultImage(int pid, int imgid)
        {
            return _DbSession.ProductStorager.SetDefaultImage(pid, imgid);
        }

        /// <summary>
        /// 获取商品图片
        /// </summary>
        /// <param name="pid">商品Id</param>
        /// <returns></returns>
        public IEnumerable<ProductImage> GetImage(int pid)
        {
            return _DbSession.ProductStorager.GetImage(pid);
        }

        /// <summary>
        /// 添加商品详情
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int AddDetail(ProductDetail entity)
        {
            return _DbSession.ProductStorager.AddDetail(entity);
        }

        /// <summary>
        /// 编辑商品详情
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool EditDetail(ProductDetail entity)
        {
            return _DbSession.ProductStorager.EditDetail(entity);
        }

        /// <summary>
        /// 获取商品详情
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        public ProductDetail GetDetail(int pid)
        {
            return _DbSession.ProductStorager.GetDetail(pid);
        }

        /// <summary>
        /// 获取某个商品的商品类型
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        public ProductCategory GetProductCategory(int pid)
        {
            return _DbSession.ProductStorager.GetProductCategory(pid);
        }

        /// <summary>
        /// 添加商品与商品类型关系
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int AddProductCategory(ProductCategory entity)
        {
            return _DbSession.ProductStorager.AddProductCategory(entity);
        }

        /// <summary>
        /// 编辑商品与商品类型关系
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool EditProductCategory(ProductCategory entity)
        {
            //判断是否存在物流信息
            bool bol = _DbSession.ProductStorager.EditProductCategory(entity);

            if (bol)
            {
                return true;
            }
            else
            {
                int id = _DbSession.ProductStorager.AddProductCategory(entity);

                if (id > 0)
                {
                    return true;
                }
            }

            return false;
        }

        /// 得到所有的产品类型
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ProductType> GetAllProductTypes()
        {
            return _DbSession.ProductStorager.GetAllProductTypes();
        }

        /// <summary>
        /// 得到所有的产品颜色
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ProductColor> GetAllProductColors()
        {
            return _DbSession.ProductStorager.GetAllProductColors();
        }


        #endregion
    }
}
