using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VcyBer.BLMS.MobileWeb.Models;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.Entity.Generated;
using Vcyber.BLMS.Entity.Enum;

namespace VcyBer.BLMS.MobileWeb.Controllers
{
    public class CarController : BaseController
    {
        //
        // GET: /Car/
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Car_Feature(string carType, string seriesName)
        {
            CarFeature carFeature = GetCarFeatureByCarType(carType);
            carFeature.SeriesName = seriesName;
            var url = GetCarVideoByCarType(carType).Url;
            ViewBag.HasVideo = url != null && url.Length > 0 ? true : false;
            return View(carFeature);
        }
        public ActionResult Car_Video(string carType, string seriesName)
        {
            CarVideo carVideo = GetCarVideoByCarType(carType);
            carVideo.SeriesName = seriesName;
            //var url = GetCarVideoByCarType(carType).Url;
            //ViewBag.HasVideo = url!=null&&url.Length > 0 ? true : false;
            return View(carVideo);
        }
        public ActionResult Car_Item(string carType, string seriesName)
        {
            CarItem carItem = GetCarItemByCarType(carType);
            carItem.SeriesName = seriesName;
            var url = GetCarVideoByCarType(carType).Url;
            ViewBag.HasVideo = url != null && url.Length > 0 ? true : false;
            return View(carItem);
        }
        public ActionResult TestDrive(string carType, string seriesName)
        {
            if (!this.User.Identity.IsAuthenticated)
            {
                //return RedirectToAction("Login", "Account",);
                return Redirect("/Account/Login?url=" + Request.RawUrl);
            }
            CarTestDrive carTestDrive = new CarTestDrive()
            {
                CarType = carType,
                SeriesName = seriesName,
                CarTypeList = _AppContext.BaseCarApp.QueryCars(ECarSeriesType.TestDrive).ToList(),
                Provinces = _AppContext.DealerApp.GetProvinceList().ToList()
            };
            return View(carTestDrive);
        }
        private CarFeature GetCarFeatureByCarType(string carType)
        {
            CarFeature carFeature = new CarFeature()
            {
                CarType = carType,
                ShopSlideUrlList = new List<string>(),
                ArticleProductList = new List<ArticleProduct>()
            };
            switch (carType)
            {
                #region Celesta
                case "Celesta":
                    {
                        carFeature.ShopSlideUrlList.Add("/img/car/Celesta/1.jpg");
                        carFeature.ShopSlideUrlList.Add("/img/car/Celesta/2.png");
                        carFeature.ShopSlideUrlList.Add("/img/car/Celesta/3.png");
                        //carFeature.ShopSlideUrlList.Add("/img/car/Yuena/3.png");
                        carFeature.ArticleProductList.Add(new ArticleProduct()
                        {
                            Title = "视觉美学──是仪式感的第1步",
                            ImgUrl = "/img/car/Celesta/4.png",
                            Content = "全新悦动采用“Modern Simplicity”现代简约外观设计理念，通过倾瀑星云的视觉效果，配合两侧的投射式星锐大灯以及两条笔直的LED日间行车灯带，打造出富有现代品质感，又凌厉大气的外观造型。内饰则采用“Wide Sphere”典雅大气的设计理念，以中央出风口为中心的水平横向内饰布局，搭配同级独有的8英寸中控触屏，带来舒展、宽阔的视觉体验。"
                        });
                        carFeature.ArticleProductList.Add(new ArticleProduct()
                        {
                            Title = "质感与智能──都时刻在线",
                            ImgUrl = "/img/car/Celesta/5.png",
                            Content = "全新悦动通过布局改善，实现最大化空间利用率，为家人营造出更舒适、人性化的乘坐感受。同级竞品中尺寸最大的中控台下方储物盘，满足用户对大屏手机等IT产品的摆放习惯；同级最大的490L后备箱容积，提供了更宽敞的储物空间；5项隔音性能的改善、7项结构刚度的强化，发动机和变速器的优化，有效提升了NVH性能。此外，后备箱智能开启、CarPlay&CarLife智能手机互联系统等同级领先智能科技装备，让更多的家庭用户得以享受便利舒适的汽车生活。"
                        });
                        carFeature.ArticleProductList.Add(new ArticleProduct()
                        {
                            Title = "治愈系x技术控──冷萌科技带来的安全感",
                            ImgUrl = "/img/car/Celesta/6.png",
                            Content = "全新悦动搭载VSM、ESC、HAC、TPMS、ESS等多种先进配置，并通过对底盘系统助力器结构优化，带来同级别竞品中最优制动感及43.4m最小制动距离。此外，全新悦动还通过扩大超高强度钢使用率、改善车身结构以及扩大侧气囊保护面积，形成完备的主被动安全防护体系，为全家带来坚实可靠的安全保障。"
                        });
                        carFeature.ArticleProductList.Add(new ArticleProduct()
                        {
                            Title = "节能+动感──不可自拔的双重吸引力",
                            ImgUrl = "/img/car/Celesta/6.png",
                            Content = "全新悦动搭载伽马1.6L D-CVVT发动机，通过VIS可变进气系统等9项发动机技术提升，实现高效动力输出，匹配同级竞品独有的6速手动和第二代6速手自一体变速器，带来同级最低的5.3L/百公里综合油耗，实现驾驶乐趣和燃油经济性的兼顾。"
                        });
                        break;
                    }
                #endregion
                #region yuena
                case "Yuena":
                    {
                        carFeature.ShopSlideUrlList.Add("/img/car/Yuena/8-1.jpg");
                        carFeature.ShopSlideUrlList.Add("/img/car/Yuena/1-1.png");
                        carFeature.ShopSlideUrlList.Add("/img/car/Yuena/2-1.png");
                        //carFeature.ShopSlideUrlList.Add("/img/car/Yuena/3.png");
                        carFeature.ArticleProductList.Add(new ArticleProduct()
                        {
                            Title = "悦目设计",
                            ImgUrl = "/img/car/Yuena/4-1.jpg",
                            Content = "这是一个看脸的世界。看不惯，就习惯我颠覆式的流体雕塑2.0设计风格和Keen-edged锋芒的外观设计；自动控制前大灯、LED日间行车灯，怎么看，我说了算；LED组合尾灯，造型时尚醒目；金属质感的镀铬前格栅，颜值爆表不服来战；蜂窝状动态前格栅，别出心裁，造型独特；锋芒16寸铝合金轮毂带你装酷，玩转世界。"
                        });
                        carFeature.ArticleProductList.Add(new ArticleProduct()
                        {
                            Title = "悦心空间",
                            ImgUrl = "/img/car/Yuena/5-1.jpg",
                            Content = "配得上大脑洞的大空间。485L超大后备箱（仅限三厢），不止于大；超帅后备箱感应自动开启（仅限三厢）；2600mm超长轴距随便躺，就当自己家，前排急速座椅加热，十秒使你热血沸腾；冷暖自知已过时，全能自动空调外加自动除雾和空气净化，比自己更疼你；电动天窗，轻轻一动仰望45度，耍帅easy。"
                        });
                        carFeature.ArticleProductList.Add(new ArticleProduct()
                        {
                            Title = "悦值配置",
                            ImgUrl = "/img/car/Yuena/6-1.jpg",
                            Content = "最新的性感代表是头脑。CarPlay&CarLife双屏互联，两全其美，玩得有模有样；后排座椅6:4分割折叠（仅限两厢）；放心玩，大胆嗨，车速感应自动落锁+碰撞自动解锁，告别危险so简单；高清倒车影像系统，后知不后觉；多功能智能钥匙手环（黑、蓝、棕潮感三色随心选）。"
                        });
                        carFeature.ArticleProductList.Add(new ArticleProduct()
                        {
                            Title = "悦享安全",
                            ImgUrl = "/img/car/Yuena/7-1.jpg",
                            Content = "分分钟Hold住全场！什么都有，不服? 高级胎压监测，先知不是传说；1.4L&1.6L发动机，配合6速手自一体/6速手动变速箱，两招制胜，秒赢；后轮碟刹，制住所有不服；VSM车辆稳定控制系统，稳操胜券，hold住一切。"
                        });
                        break;
                    }
                #endregion
                #region SonataNinePHEV
                case "SonataNinePHEV":
                    {
                        carFeature.ShopSlideUrlList.Add("/img/car/SonataNinePHEV/s_1.jpg");
                        carFeature.ShopSlideUrlList.Add("/img/car/SonataNinePHEV/s_2.jpg");
                        carFeature.ShopSlideUrlList.Add("/img/car/SonataNinePHEV/s_3.jpg");
                        carFeature.ArticleProductList.Add(new ArticleProduct()
                        {
                            Title = "动力",
                            ImgUrl = "/img/car/SonataNinePHEV/p_1.jpg",
                            Content = "既能风驰电掣，也能美丽地球，两全其美的hybrid。其搭载 2.0L GDi Engine（Nu）发动机和38kw electric motor 电动机，采用并联的混合动力系统连接方式，提供最佳动力的同时保证了燃油经济型，相对同排量燃油车节油36.8%，百公里油耗仅4.8升。同时6速手自一体变速器、多种驾驶模式选择，给您带来完美的驾驶体验。"
                        });
                        carFeature.ArticleProductList.Add(new ArticleProduct()
                        {
                            Title = "外观",
                            ImgUrl = "/img/car/SonataNinePHEV/p_2.jpg",
                            Content = "懂得欣赏，也得懂得自我欣赏。流体雕塑2.0设计理念、LED日间行车灯、LED后组合尾灯、全景天窗、时尚大气前格栅、远近光一体式HID氙气大灯、电动调节/加热/折叠外后视镜、17寸轮毂以及扰流板的完美搭配，由内而外散发出强劲魄力，狂刷存在感。"
                        });
                        carFeature.ArticleProductList.Add(new ArticleProduct()
                        {
                            Title = "舒适",
                            ImgUrl = "/img/car/SonataNinePHEV/p_3.jpg",
                            Content = "满足一切所需，及时享受。后备箱智能开启、ASCC智能自适应巡航、前排座椅通风/前后排座椅加热、后排多功能豪华中央扶手、双区独立控制自动空调、超级仪表盘、驾驶席电动座椅12向调节（带腰部支撑）、前排座椅通风、前后排座椅加热，智能化、人性化的提升您的驾驭体验。"
                        });
                        carFeature.ArticleProductList.Add(new ArticleProduct()
                        {
                            Title = "安全",
                            ImgUrl = "/img/car/SonataNinePHEV/p_4.jpg",
                            Content = "智在智乐的安全感，才有感。包围式7气囊、LDWS车道偏离警示系统、超高张力钢板应用率达51%、TPMS胎压监测系统、膝部气囊、电子防炫目内后视镜，将带给您更为智能化的安全感。"
                        });
                        break;
                    }
                #endregion
                #region NewElantra
                case "NewElantra":
                    {
                        carFeature.ShopSlideUrlList.Add("/img/car/NewElantra/s_1.jpg");
                        carFeature.ShopSlideUrlList.Add("/img/car/NewElantra/s_2.jpg");
                        carFeature.ShopSlideUrlList.Add("/img/car/NewElantra/s_3.jpg");
                        carFeature.ArticleProductList.Add(new ArticleProduct()
                        {
                            Title = "外观",
                            ImgUrl = "/img/car/NewElantra/p_1.jpg",
                            Content = "颜值才是王道。新朗动配备了0.29风阻系数的车身设计、蜂窝式前格栅、投射式前大灯/LED日间行车灯、大视野外后视镜、17″铝合金轮毂，华丽转身，颜值爆棚。"
                        });
                        carFeature.ArticleProductList.Add(new ArticleProduct()
                        {
                            Title = "空间",
                            ImgUrl = "/img/car/NewElantra/p_2.jpg",
                            Content = "用心才有品质。空气净化系统，空调离子发生器、驾驶席座椅通风、前排座椅加热等使您不论何时何地，皆能享受舒适清新的智能空间。"
                        });
                        carFeature.ArticleProductList.Add(new ArticleProduct()
                        {
                            Title = "动力",
                            ImgUrl = "/img/car/NewElantra/p_3.jpg",
                            Content = "有料才敢放肆。在内饰方面新朗动搭载了ISG发动机自动启停、自动助力转向、6速手自一体变速器、1.6Lγ发动机，为您提供持续强劲动力的同时，在操控性能上也可张可驰。"
                        });
                        carFeature.ArticleProductList.Add(new ArticleProduct()
                        {
                            Title = "安全",
                            ImgUrl = "/img/car/NewElantra/p_4.jpg",
                            Content = "更好的安全需要更多的保障。新朗动配备了倒车影像、VSM车辆稳定控制系统、TPMS胎压监测系统、超高张力钢板吸能车身等，让您的爱车时时刻刻保卫您的安全。"
                        });
                        carFeature.ArticleProductList.Add(new ArticleProduct()
                        {
                            Title = "配置",
                            ImgUrl = "/img/car/NewElantra/p_5.jpg",
                            Content = "要玩就要玩出不一样。新朗动在智能配置方面依旧惹人喜爱，他配备的超级仪表盘、智能钥匙一键启动、方向盘加热、CarPlay&CarLife手机互联（IOS和Android系统手机均可使用）等，让你时刻玩出彩。"
                        });
                        break;
                    }
                #endregion
                #region lingdong
                case "lingdong":
                    {
                        carFeature.ShopSlideUrlList.Add("/img/car/lingdong/1.png");
                        carFeature.ShopSlideUrlList.Add("/img/car/lingdong/2.png");
                        carFeature.ShopSlideUrlList.Add("/img/car/lingdong/3.png");
                        carFeature.ArticleProductList.Add(new ArticleProduct()
                        {
                            Title = "领风芒，动倾心",
                            ImgUrl = "/img/car/lingdong/1.jpg",
                            Content = "不追风，不表象，基于根本，回归本质。在造型上领动整体外形采用流体雕塑2.0理念，同时还配备了风尚投射式前大灯、创新LED侧转向灯、L型雾灯+独立转向辅助照明灯、运动式镀铬双排气管、多功能LED后组合尾灯、独创六边形镀铬格栅，使其更显气质非凡。"
                        });
                        carFeature.ArticleProductList.Add(new ArticleProduct()
                        {
                            Title = "领时代，动于心",
                            ImgUrl = "/img/car/lingdong/2.jpg",
                            Content = "不喧哗，不浮夸，基于根本，专注本源。智能配置上，领动在内饰方面搭载了前后排座椅加热功能给您舒适的驾享体验；方向盘加热+D-Cut运动方向盘，皮包裹的装饰提供自然握感及操作感，彰显高档质感，方向盘集成的多功能按钮，更加方便驾驶者操作；智能钥匙长按上锁键即可远程遥控关闭四门车窗；8英寸智能LCD显示屏+CarPlay&CarLife手机互联,满足不同手机端的使用条件，实现导航/电话/信息/音乐等的车辆多媒体功能；驾驶席&副驾驶席通风座椅以及后备箱智能开启等多项智能技术，亦可为您增添一份舒适与安逸。"
                        });
                        carFeature.ArticleProductList.Add(new ArticleProduct()
                        {
                            Title = "领内在，动极速",
                            ImgUrl = "/img/car/lingdong/3.jpg",
                            Content = "不征服，不改变，基于根本，互敬平衡。领动稳扎稳打的不只是动力，更是魅力。领动配备了运动型1.6L GDi发动机+6速手自一体、强劲1.4T-GDi发动机+7速双离合变速器，为整车提供源源不断磅礴动力的同时，在操控性能上也可张可驰。"
                        });
                        carFeature.ArticleProductList.Add(new ArticleProduct()
                        {
                            Title = "领安全，动安心",
                            ImgUrl = "/img/car/lingdong/4.jpg",
                            Content = "不追逐，不冲动，基于根本，坚守原则。无论何时，无论何地，全方位的保护始终如影随形。53%超高强度钢板、以及包围式6+1安全气囊、LDWS车道偏离报警、BSD盲区监测以及AED自动紧急制动系统，将安全融入每一次旅程。"
                        });
                        break;
                    }
                #endregion
                #region Sonata9_Characteristic
                case "Sonata9_Characteristic":
                    {
                        carFeature.ShopSlideUrlList.Add("/img/car/Sonata9/1.jpg");
                        carFeature.ShopSlideUrlList.Add("/img/car/Sonata9/2.jpg");
                        carFeature.ArticleProductList.Add(new ArticleProduct()
                        {
                            Title = "有索见",
                            ImgUrl = "/img/car/Sonata9/Sonata9_c_04.jpg",
                            Content = "有一种锋芒，未动已直抵内心深处。初见时心动，回眸间驭动。第九代索纳塔的设计是时尚和科技的完美结合。流体雕塑2.0设计理念，展现精致流体美学。风阻系数0.27，流线型的车身设计，带来完美的行驶表现。远近光一体式氙气大灯，尽显锐利与稳重。LED日间行车灯，点亮平凡之路。"
                        });
                        carFeature.ArticleProductList.Add(new ArticleProduct()
                        {
                            Title = "有索享",
                            ImgUrl = "/img/car/Sonata9/Sonata9_c_01.jpg",
                            Content = "有一片空间，容得下心中所有想象。置身之内，享驭未来杰作。第九代索纳塔尊享高科技内部空间，集宽敞和舒适于一体，营造最佳驾乘感受。D-Cut方向盘，采用人机工程学设计，展现同级最佳握感。前排通风座椅，为后排乘客提供舒适的乘车环境。后排多功能豪华中央扶手，舒适、便捷、高档，展现高端车的独有豪华。"
                        });
                        carFeature.ArticleProductList.Add(new ArticleProduct()
                        {
                            Title = "有索动",
                            ImgUrl = "/img/car/Sonata9/Sonata9_c_02.jpg",
                            Content = "有一束风芒，颠覆速度的极限。T动力时代，势为越己者驾临！7速双离合变速器，技术先进，反应速度更快，与1.6T-GDi、2.4L-GDi发动机完美匹配，打造狂野动力输出，提升驾阅激情。"
                        });
                        carFeature.ArticleProductList.Add(new ArticleProduct()
                        {
                            Title = "有索顾",
                            ImgUrl = "/img/car/Sonata9/Sonata9_c_03.jpg",
                            Content = "有一份周全，由内而外全心追随。以非凡前瞻洞察，悦享征程。第九代索纳塔高刚性安全车身，能有效保护车内乘员安全。七安全气囊，真正的全方位安全防护，让驾乘更加安心。TPMS独立数显胎压监测，全时不间断监控，让行车更加安心。"
                        });
                        carFeature.ArticleProductList.Add(new ArticleProduct()
                        {
                            Title = "有索感",
                            ImgUrl = "/img/car/Sonata9/Sonata9_c_04.jpg",
                            Content = "有一种智慧，视未来为现在。魅力科技，慧引时代向前。ASCC智能自适应巡航，高科技、全自动，让高速行驶更加轻松便捷。LDWS车道偏离警示系统，预防危险保障安全。blue link智能人车交互系统，让生活更加简单便捷。blue explorer，人、车、智能手机融为一体，源于人性化思考。SPAS智能泊车系统，高档、高科技的体现，让用车更加便捷。"
                        });
                        break;
                    }
                #endregion
                #region IX25_Characteristic
                case "IX25_Characteristic":
                    {
                        carFeature.ShopSlideUrlList.Add("/img/car/IX25/sonota9_0011.jpg");
                        carFeature.ShopSlideUrlList.Add("/img/car/IX25/sonota9_0012.jpg");
                        carFeature.ArticleProductList.Add(new ArticleProduct()
                        {
                            Title = "耀青春－炫目锋尚外观",
                            ImgUrl = "/img/car/IX25/sonota9_0011.jpg",
                            Content = "ix25在外观设计上运用了流体雕塑2.0设计理念，将强硬的气息与流行的元素完美融合。搭配豪华全景天窗、悬浮式座舱以及双五幅轮毂组合出极具现代感和吸引力的动感时尚外观。 投射式大灯、LED日间行车灯、LED后组合尾灯、高亮度光源前雾灯彰显炫目锋尚的个性，体现卓尔不群的身份。"
                        });
                        carFeature.ArticleProductList.Add(new ArticleProduct()
                        {
                            Title = "智青春－超前极致体验",
                            ImgUrl = "/img/car/IX25/ix25_c_02.jpg",
                            Content = "ix25的内饰设计将青春与力量完美结合，将质感提升到极致，给人温馨如家的感觉。凭借2590mm同级别领先轴距，造就了极为宽适的内部空间，优越的后备箱空间、座椅底部储物盒等空间设计充分满足了车内储物需求。一键启动、智能钥匙、超级仪表盘、7寸超大液晶显示屏等智能细节配置，由内而外散发着科技气息。前排加热座椅、多档位通风座椅、全自动高效空调，后排出风口等人性化功能，让每一次驾驶都充满期待。"
                        });
                        carFeature.ArticleProductList.Add(new ArticleProduct()
                        {
                            Title = "享青春－创领智能安全",
                            ImgUrl = "/img/car/IX25/ix25_c_03.jpg",
                            Content = "ix25将安全设计理念提到新的巅峰。无论是倒车影像、自动防眩目内后视镜、HAC上坡起步辅助系统、DBC下坡刹车辅助系统、智能转向辅助照明系统此类先进可靠的主动安全电子设备，还是超高强度钢板构成的强化车身机构、全方位6安全气囊所来带稳如泰山的被动安全保护，都能展现ix25驾驶者源自内心深处的从容。"
                        });
                        carFeature.ArticleProductList.Add(new ArticleProduct()
                        {
                            Title = "悦青春－释放非凡动力",
                            ImgUrl = "/img/car/IX25/ix25_c_04.jpg",
                            Content = "1.6L Gamma和2.0L Nu两款自然吸气黄金动力组合发动机，带给ix25强劲灵动的性能、加速推背感、随心的驾控欲还有燃油经济性。6速变速箱让每一个动作、每一滴燃油都充分的尽职尽责去完成它们的工作，给予驾驶者无与伦比的驾驶体验。"
                        });
                        break;
                    }
                #endregion
                #region Santafe_Characteristic
                case "Santafe_Characteristic":
                    {
                        carFeature.ShopSlideUrlList.Add("/img/car/Santafe/1.jpg");
                        carFeature.ShopSlideUrlList.Add("/img/car/Santafe/2.jpg");
                        carFeature.ArticleProductList.Add(new ArticleProduct()
                        {
                            Title = "魅动外观",
                            ImgUrl = "/img/car/Santafe/Santafe_c_01.jpg",
                            Content = "设计师从无形的风暴中吸取创意灵感，创造性地将风暴前沿设计应用于魅动外观，令整车展现灵动大气的视觉感官；更有大气高档的双排气管、氙气大灯+日间行车灯、18寸时尚轮毂等创意设计，让整个外观更显气势非凡。"
                        });
                        carFeature.ArticleProductList.Add(new ArticleProduct()
                        {
                            Title = "宽适空间",
                            ImgUrl = "/img/car/Santafe/Santafe_c_02.jpg",
                            Content = "全新胜达的2700mm超长轴距、7座豪华灵动空间、全视角全景天窗营造出超大豪华空间, 为驾乘者带来无与伦比的舒适体验，心境宽广至臻。"
                        });
                        carFeature.ArticleProductList.Add(new ArticleProduct()
                        {
                            Title = "智能配置",
                            ImgUrl = "/img/car/Santafe/Santafe_c_03.jpg",
                            Content = "面对复杂多变的外在世界，让冰冷的科技化身为内在舒适的体验，是全新胜达的睿智所在。其自动泊车（SPAS）、智能人车交互系统（BLUE LINK）、随动转向氙气大灯（HID）等科技应用令整车更智能，完美匹配人之所需。"
                        });
                        carFeature.ArticleProductList.Add(new ArticleProduct()
                        {
                            Title = "人性化设计",
                            ImgUrl = "/img/car/Santafe/Santafe_c_04.jpg",
                            Content = "除丰富的智能配置外，全新胜达还拥有多项人性化配置：记忆座椅、双区自动空调、通风座椅等，处处体现以人为本的人性化设计理念。"
                        });
                        carFeature.ArticleProductList.Add(new ArticleProduct()
                        {
                            Title = "磅礴动力",
                            ImgUrl = "/img/car/Santafe/Santafe_c_05.jpg",
                            Content = "纵情驰骋的力量源自内在能量。全新胜达配备2.0TGDI缸内直喷涡轮增压发动机、6速手自一体变速器、柔性转向系统（FLEX STEER）及智能电子四驱系统，为整车提供源源不断的磅礴动力和可张可驰的操控性能。"
                        });
                        carFeature.ArticleProductList.Add(new ArticleProduct()
                        {
                            Title = "安全保障",
                            ImgUrl = "/img/car/Santafe/Santafe_c_06.jpg",
                            Content = "纵然面对曲折，亦敢勇往直前。纵然险况环生，也能从容以对。全新胜达拥有车道偏离警示系统（LDWS）、高级胎压监测、6安全气囊、电子手刹（EPB）+自动驻车（AUTO HOLD），让驾驶者一路纵情驰骋的同时安然无忧。"
                        });
                        break;
                    }
                #endregion
                #region Langdong_Characteristic
                case "Langdong_Characteristic":
                    {
                        carFeature.ShopSlideUrlList.Add("/img/car/Langdong/1.jpg");
                        carFeature.ShopSlideUrlList.Add("/img/car/Langdong/2.jpg");
                        carFeature.ArticleProductList.Add(new ArticleProduct()
                        {
                            Title = "酷炫外观",
                            ImgUrl = "/img/car/Langdong/Langdong_c_01.jpg",
                            Content = "流体雕塑—流水的灵性、柔美感和山川雕塑的强劲力量感完美结合在一起，以自然风体流动的神造之笔作为“朗动”的设计主旋律。"
                        });
                        carFeature.ArticleProductList.Add(new ArticleProduct()
                        {
                            Title = "舒适空间",
                            ImgUrl = "/img/car/Langdong/Langdong_c_02.jpg",
                            Content = "朗动车身内部设计采用实用空间最大化理念打造，2700mm超长轴距，不论前排后排，都能获得自由伸展的空间，真正做到最合理化设计。"
                        });
                        carFeature.ArticleProductList.Add(new ArticleProduct()
                        {
                            Title = "丰富配置",
                            ImgUrl = "/img/car/Langdong/Langdong_c_03.jpg",
                            Content = "智能钥匙一键启动、定速巡航、方向盘换挡拨片、驾驶席通风座椅、前排电加热座椅、后排空调出风口、双区独立自动空调带离子发生器等豪华配置，让驾乘者享受舒适旅途同时，充分感受驾驶朗动所带来的乐趣。"
                        });
                        carFeature.ArticleProductList.Add(new ArticleProduct()
                        {
                            Title = "澎湃动力",
                            ImgUrl = "/img/car/Langdong/Langdong_c_04.jpg",
                            Content = "朗动搭载重新调校的1.6Lγ发动机以及全新的1.8L Nu 发动机，采用6速手自一体、主动式ECO经济驾驶模式，使动力和燃油经济性处于领先位置。"
                        });
                        carFeature.ArticleProductList.Add(new ArticleProduct()
                        {
                            Title = "智能安全",
                            ImgUrl = "/img/car/Langdong/Langdong_c_05.jpg",
                            Content = "驾乘朗动，享受周到的安全保障。朗动配备6安全气囊、前后驻车雷达、TPMS胎压检测系统及ESP/VSM车身稳定控制系统，为驾乘者提供全方位的主/被动安全。"
                        });
                        break;
                    }
                #endregion
                #region Mistra
                case "Mistra":
                    {
                        carFeature.ShopSlideUrlList.Add("/img/car/Mistra/s_1.jpg");
                        carFeature.ShopSlideUrlList.Add("/img/car/Mistra/s_2.jpg");
                        carFeature.ShopSlideUrlList.Add("/img/car/Mistra/s_3.jpg");
                        carFeature.ArticleProductList.Add(new ArticleProduct()
                        {
                            Title = "重塑潮流趋势",
                            ImgUrl = "/img/car/Mistra/p_1.jpg",
                            Content = "以创所未有的设计，重塑潮流趋势。采用风暴前沿设计，鹰眼式氙气大灯、炫目LED日间行车灯、锋芒LED组合尾灯，在保证安全性的同时，提升车头及车尾的视觉美感。"
                        });
                        carFeature.ArticleProductList.Add(new ArticleProduct()
                        {
                            Title = "颠覆驾享模式",
                            ImgUrl = "/img/car/Mistra/p_2.jpg",
                            Content = "以超乎想象的体验，颠覆驾享模式。名图配备了智能一键启动、Nu 2.0L科技领先发动机、多连杆式独立后悬架以及三段式柔性转向系统，能够充分满足消费者的驾享体验。"
                        });
                        carFeature.ArticleProductList.Add(new ArticleProduct()
                        {
                            Title = "领先智能安全",
                            ImgUrl = "/img/car/Mistra/p_3.jpg",
                            Content = "以全方位的保护，领先智能安全。名图采用多级燃爆安全气囊，高强度车身结构、HAC上坡辅助系统，TPMS胎压监测系统，VSM电子车身稳定系统等为您的安全保驾护航。"
                        });
                        carFeature.ArticleProductList.Add(new ArticleProduct()
                        {
                            Title = "定义未来科技",
                            ImgUrl = "/img/car/Mistra/p_4.jpg",
                            Content = "以赶超时代的配置，定义未来科技。名图配备了广角全尺寸全景天窗、车载空气净化器、超静音室内环境、7英寸大屏车载智能互联多媒体，多项科技配置全方位提升驾驶者的体验感。"
                        });
                        break;
                    }
                #endregion
                #region Verna_Characteristic
                case "Verna_Characteristic":
                    {
                        carFeature.ShopSlideUrlList.Add("/img/car/Verna/1.jpg");
                        carFeature.ShopSlideUrlList.Add("/img/car/Verna/2.jpg");
                        carFeature.ArticleProductList.Add(new ArticleProduct()
                        {
                            Title = "时尚外观",
                            ImgUrl = "/img/car/Verna/Verna_c_01.jpg",
                            Content = "以气质迷人，你就是潮流的代言人。新 瑞纳/瑞奕拥有锐利投射式前大灯、LED绚丽尾灯（瑞纳）、前卫近气格栅，让整车极具动感；双开启式电动天窗有效提高车内采光和通风效果，同时给你更多自由空间。"
                        });
                        carFeature.ArticleProductList.Add(new ArticleProduct()
                        {
                            Title = "灵动操控",
                            ImgUrl = "/img/car/Verna/Verna_c_02.jpg",
                            Content = "左右自如，向前的路才够好玩！新 瑞纳/瑞奕配置全面升级：电动助力转向系统让行车更省力；多功能皮质方向盘，数控科技按键，让驾驶者在行驶过程中轻松享受驾乘乐趣；智能行车电脑，全面提升驾驶安全性；中高配车款配备四轮盘刹，能够轻松适应各种路况，安全舒适，收放自如！"
                        });
                        carFeature.ArticleProductList.Add(new ArticleProduct()
                        {
                            Title = "超级配置",
                            ImgUrl = "/img/car/Verna/Verna_c_03.jpg",
                            Content = "Smart空间，让生活充满surprise！新 瑞纳/瑞奕配备智能钥匙一键启动、发动机电子防盗系统，让驾驶者全方位感受智能化的安全便捷，全面开启精彩驾乘体验；ESS紧急制动提醒系统，急刹车时制动灯闪烁，提升行驶安全性；皮质人体工程学座椅（瑞纳）尽显人性化设计，白黑个性内饰（瑞奕）尽显智慧搭配美学，简洁时尚独具个性，为驾乘人员提供舒适、时尚的驾乘体验。"
                        });
                        break;
                    }
                #endregion
                #region Ix35_Characteristic
                case "Ix35_Characteristic":
                    {
                        carFeature.ShopSlideUrlList.Add("/img/car/ix35/1.jpg");
                        carFeature.ShopSlideUrlList.Add("/img/car/ix35/2.jpg");
                        carFeature.ArticleProductList.Add(new ArticleProduct()
                        {
                            Title = "时尚外观",
                            ImgUrl = "/img/car/ix35/ix35_c_01.jpg",
                            Content = "眼界不同，场合不同，角度不同，对美的答案却是同一个；时尚大气的新格栅、镀铬腰线、锋芒LED炫目尾灯、豪华全景天窗，ix35的外观动感而时尚，以惊鸿一瞥的线条和创新美丽的灯光，俘获万众。"
                        });
                        carFeature.ArticleProductList.Add(new ArticleProduct()
                        {
                            Title = "丰富配置",
                            ImgUrl = "/img/car/ix35/ix35_c_02.jpg",
                            Content = "乐趣之源，ix35完美操控，TA是把所有的路都平坦顺畅的魔法师。智能自动泊车辅助系统、智能七寸导航系统、舒适前排座椅加热、车载空气净化器、外后镜加热等多项豪华配置，提供驾驶者舒适的驾控体验。"
                        });
                        carFeature.ArticleProductList.Add(new ArticleProduct()
                        {
                            Title = "澎湃动力",
                            ImgUrl = "/img/car/ix35/ix35_c_03.jpg",
                            Content = "感官之享，ix35豪华配置，让新锐的您更精睿。ix35搭载强劲2.0L NU发动机、顺畅6速手自一体变速器、智能一键启动系统、适时四轮驱动系统，以无可阻挡澎湃动力，游刃于城市，是精英的移动SOHO，也是最舒享的SPA天堂。"
                        });
                        break;
                    }
                #endregion
                #region Tucson_Characteristic
                case "Tucson_Characteristic":
                    {
                        carFeature.ShopSlideUrlList.Add("/img/car/Tucson/1.jpg");
                        carFeature.ShopSlideUrlList.Add("/img/car/Tucson/2.jpg");
                        carFeature.ArticleProductList.Add(new ArticleProduct()
                        {
                            Title = "动感外观",
                            ImgUrl = "/img/car/Tucson/Tucson_c_01.jpg",
                            Content = "凭借战斧式个性轮毂、全新投射式前大灯、动感前卫进气格栅、精致典雅的尾灯群等18项高端品质升级，让途胜整体焕发时尚质感。"
                        });
                        carFeature.ArticleProductList.Add(new ArticleProduct()
                        {
                            Title = "灵动空间",
                            ImgUrl = "/img/car/Tucson/Tucson_c_02.jpg",
                            Content = "精益求精的态度在途胜得到充分展现，它拥有可变式座椅灵活组合：后排6:4可拆分折叠座椅以及后排座椅折叠隐没，将空间变到极致；更有可单独开启的后备厢风挡玻璃、电动天窗等丰富配置，让驾乘者享受全方位舒适体验。"
                        });
                        carFeature.ArticleProductList.Add(new ArticleProduct()
                        {
                            Title = "强劲动力",
                            ImgUrl = "/img/car/Tucson/Tucson_c_03.jpg",
                            Content = "H-Matic手自一体变速器，无论崎岖山路还是平坦公路，都能自由切换；电子智能适时四驱系统，根据路面变化，提供相应的源动力；麦弗逊式悬挂，加之充气式减震器，带来轿车般的舒适体验；2.0L-DOHC CVVT强悍发动机，任何时刻都能灵动自如。"
                        });
                        carFeature.ArticleProductList.Add(new ArticleProduct()
                        {
                            Title = "智能安全",
                            ImgUrl = "/img/car/Tucson/Tucson_c_04.jpg",
                            Content = "途胜整车采用高刚性车身结构，提高车身抵抗弯曲和扭曲变形能力；ABS+EBD在刹车时迅速调节制动压力，合理分配制动力，使车辆平稳移动；智能倒车雷达通过感应装置，自动侦测车后障碍物，迅速将信息反馈给驾驶者，保证驻车安全。"
                        });
                        break;
                    }
                #endregion
                #region Elantrayuedong_Characteristic
                case "Elantrayuedong_Characteristic":
                    {
                        carFeature.ShopSlideUrlList.Add("/img/car/Elantrayuedong/1.jpg");
                        carFeature.ShopSlideUrlList.Add("/img/car/Elantrayuedong/2.jpg");
                        carFeature.ArticleProductList.Add(new ArticleProduct()
                        {
                            Title = "丰富配置",
                            ImgUrl = "/img/car/Elantrayuedong/Elantrayuedong_c_01.jpg",
                            Content = "悦动秉承苛求完美的一贯理念，配备智能DVD导航系统、符合人体工程学舒适座椅和多功能方向盘等多项功能，让驾驶者感受新悦动带来的豪华配置。"
                        });
                        carFeature.ArticleProductList.Add(new ArticleProduct()
                        {
                            Title = "澎湃动力",
                            ImgUrl = "/img/car/Elantrayuedong/Elantrayuedong_c_02.jpg",
                            Content = "拥有低风阻流线外形的悦动将动力性能提升至全新高度，并搭载CVVT连续可变气门正时系统、智能阶梯式4档自动变速箱及ECO经济驾驶模式等诸多功能，在保持澎湃动力的同时，有效利用燃油，让驾驶者享受更高效的驾驶体验。"
                        });
                        carFeature.ArticleProductList.Add(new ArticleProduct()
                        {
                            Title = "安全保障",
                            ImgUrl = "/img/car/Elantrayuedong/Elantrayuedong_c_03.jpg",
                            Content = "纵有未知路况，也可安枕无忧；纵有险况横生，亦能化险为夷。ABS+EBD、前排侧气囊、七重加固高刚车身成就悦动无与伦比的坚固壁垒，给驾乘者带来最细致呵护的关爱。"
                        });
                        break;
                    }
                #endregion
                #region Sonata8_Characteristic
                case "Sonata8_Characteristic":
                    {
                        carFeature.ShopSlideUrlList.Add("/img/car/Sonata8/1.jpg");
                        carFeature.ShopSlideUrlList.Add("/img/car/Sonata8/2.jpg");
                        carFeature.ArticleProductList.Add(new ArticleProduct()
                        {
                            Title = "大气外观",
                            ImgUrl = "/img/car/Sonata8/Sonata8_01.jpg",
                            Content = "采用“流体雕塑”设计语言的2014款第八代索纳塔，拥有日间行车灯，前格栅质感升级，以及全新17”轮毂，整体车身尽显动感之美。"
                        });
                        carFeature.ArticleProductList.Add(new ArticleProduct()
                        {
                            Title = "宽适空间",
                            ImgUrl = "/img/car/Sonata8/Sonata8_02.jpg",
                            Content = "2014款第八代索纳塔拥有2795mm超长轴距，营造舒适空间；超大后背箱能够满足驾乘人员全面需求；黑色钢琴烤漆装饰将奢华内饰设计进行到底，豪华大型全景天窗，采用3片式设计，使内部视野最大化。"
                        });
                        carFeature.ArticleProductList.Add(new ArticleProduct()
                        {
                            Title = "豪华配置",
                            ImgUrl = "",
                            Content = "对每一处细节的精雕细琢，只因您对完美的苛求从未停歇。2014款第八代索纳塔拥有前后排座椅加热、双区独立自动空调、智能一键启动系统、Infinity音响、倒车影像系统、头等舱式通风座椅、bluelink车载信息服务系统等丰富配备，让驾乘感受进入一个全新时代。"
                        });
                        carFeature.ArticleProductList.Add(new ArticleProduct()
                        {
                            Title = "澎湃动力",
                            ImgUrl = "/img/car/Sonata8/Sonata8_03.jpg",
                            Content = "纵情驰骋与自如掌控，融汇全新动力科技的2014款第八代索纳塔让您二者兼得。2.4L θ-II 双CVVT发动机配合6速手自一体变速器及换挡拨片，更有主动式ECO经济驾驶模式助驾驶者全程掌控，出众的驾控表现，成就驾驶者激情澎湃的最终欲望。"
                        });
                        carFeature.ArticleProductList.Add(new ArticleProduct()
                        {
                            Title = "智能安全",
                            ImgUrl = "/img/car/Sonata8/Sonata8_04.jpg",
                            Content = "2014款第八代索纳塔还配备诸多智能安全配置，VSM车辆稳定控制系统、HAC上坡辅助系统、TPMS胎压监测系统、LED组合尾灯等，将智能行车理念上升至全新高度，全面呵护驾乘安全。"
                        });
                        break;
                    }
                #endregion
                default: break;
            }
            return carFeature;
        }
        private CarVideo GetCarVideoByCarType(string carType)
        {
            CarVideo carVideo = new CarVideo()
            {
                CarType = carType
            };
            switch (carType)
            {
                #region yuena
                case "Yuena":
                    {
                        carVideo.Type = "video/mp4";
                        carVideo.Url = "/img/car/Yuena/v1_verna.mp4";
                        break;
                    }
                #endregion
                #region SonataNinePHEV
                case "SonataNinePHEV":
                    {
                        carVideo.Type = "";
                        carVideo.Url = "";
                        break;
                    }
                #endregion
                #region NewElantra
                case "NewElantra":
                    {
                        carVideo.Type = "";
                        carVideo.Url = "";
                        break;
                    }
                #endregion
                #region lingdong
                case "lingdong":
                    {
                        carVideo.Type = "";
                        //carVideo.Url = "/img/car/New_Tucson/2.mp4";
                        break;
                    }
                #endregion
                #region Sonata9_Characteristic
                case "Sonata9_Characteristic":
                    {
                        carVideo.Type = "video/mp4";
                        carVideo.Url = "http://hbweixin.maimang.net.cn/bx_dingyue/video/suo9shangshi.mp4";
                        break;
                    }
                #endregion
                #region IX25_Characteristic
                case "IX25_Characteristic":
                    {
                        carVideo.Type = "";
                        carVideo.Url = "";
                        break;
                    }
                #endregion
                #region Santafe_Characteristic
                case "Santafe_Characteristic":
                    {
                        carVideo.Type = "";
                        carVideo.Url = "http://hbweixin.maimang.net.cn/bx_dingyue/video/shengdafei03.mp4";
                        break;
                    }
                #endregion
                #region Langdong_Characteristic
                case "Langdong_Characteristic":
                    {
                        carVideo.Type = "";
                        carVideo.Url = "";
                        break;
                    }
                #endregion
                #region Mistra
                case "Mistra":
                    {
                        carVideo.Type = "";
                        carVideo.Url = "";
                        break;
                    }
                #endregion
                #region Verna_Characteristic
                case "Verna_Characteristic":
                    {
                        carVideo.Type = "";
                        carVideo.Url = "";
                        break;
                    }
                #endregion
                #region Ix35_Characteristic
                case "Ix35_Characteristic":
                    {
                        carVideo.Type = "";
                        carVideo.Url = "";
                        break;
                    }
                #endregion
                #region Tucson_Characteristic
                case "Tucson_Characteristic":
                    {
                        carVideo.Type = "";
                        carVideo.Url = "";
                        break;
                    }
                #endregion
                #region Elantrayuedong_Characteristic
                case "Elantrayuedong_Characteristic":
                    {
                        carVideo.Type = "";
                        carVideo.Url = "";
                        break;
                    }
                #endregion
                #region Sonata8_Characteristic
                case "Sonata8_Characteristic":
                    {
                        carVideo.Type = "video/mp4";
                        carVideo.Url = "http://hbweixin.maimang.net.cn/bx_dingyue/video/luoyepian_30Sec_final_0317.mp4";
                        break;
                    }
                #endregion
                default: break;
            }
            return carVideo;
        }
        private CarItem GetCarItemByCarType(string carType)
        {
            CarItem carItem = new CarItem()
            {
                CarType = carType,
                CarPriceList = new List<CarPrice>(),
                CarPrices= new List<CarPrice>()
            };
            switch (carType)
            {

                #region Celesta
                case "Celesta":
                    {
                        carItem.CarPriceList.Add(new CarPrice()
                        {
                            Name = "1.6 手动悦目版",
                            Price = "79,900"
                        });
                        carItem.CarPriceList.Add(new CarPrice()
                        {
                            Name = "1.6 手动悦值版",
                            Price = "91,900"
                        });
                        carItem.CarPriceList.Add(new CarPrice()
                        {
                            Name = "1.6 手动悦心版",
                            Price = "101,900"
                        });
                        carItem.CarPriceList.Add(new CarPrice()
                        {
                            Name = "1.6 自动悦值版",
                            Price = "102,900"
                        });
                        carItem.CarPriceList.Add(new CarPrice()
                        {
                            Name = "1.6 自动悦心版",
                            Price = "115,900"
                        });

                        break;
                    }
                #endregion
                #region yuena
                case "Yuena":
                    {
                        carItem.CarPriceList.Add(new CarPrice()
                        {
                            Name = "1.4 GS MT  青春版",
                            Price = "72,800"
                        });
                        carItem.CarPriceList.Add(new CarPrice()
                        {
                            Name = "1.4 GLS MT  炫酷版",
                            Price = "77,800"
                        });
                        carItem.CarPriceList.Add(new CarPrice()
                        {
                            Name = "1.4 GLS AT  炫酷版",
                            Price = "87,800"
                        });
                        carItem.CarPriceList.Add(new CarPrice()
                        {
                            Name = "1.4 SPORT AT  活力版",
                            Price = "90,800"
                        });
                        carItem.CarPriceList.Add(new CarPrice()
                        {
                            Name = "1.4 TOP AT  精英版",
                            Price = "96,800"
                        });
                        carItem.CarPriceList.Add(new CarPrice()
                        {
                            Name = "1.6 TOP AT  精英版",
                            Price = "105,800"
                        });
                        carItem.CarPrices.Add(new CarPrice()
                        {
                            Name = "1.4 GLS MT  炫酷版",
                            Price = "78,800"
                        });
                        carItem.CarPrices.Add(new CarPrice()
                        {
                            Name = "1.4 GLS AT  炫酷版",
                            Price = "88,800"
                        });
                        carItem.CarPrices.Add(new CarPrice()
                        {
                            Name = "1.4 SPORT AT  活力版",
                            Price = "94,800"
                        });
                        carItem.CarPrices.Add(new CarPrice()
                        {
                            Name = "1.6 SPORT AT  活力版",
                            Price = "99,800"
                        });
                        carItem.CarPrices.Add(new CarPrice()
                        {
                            Name = "1.6 TOP AT  精英版",
                            Price = "108,800"
                        });
                        break;
                    }
                #endregion
                #region SonataNinePHEV
                case "SonataNinePHEV":
                    {
                        carItem.CarPriceList.Add(new CarPrice()
                        {
                            Name = "2.0 HE 智能型",
                            Price = "209,800"
                        });
                        carItem.CarPriceList.Add(new CarPrice()
                        {
                            Name = "2.0 HS 领先型",
                            Price = "229,800"
                        });
                        carItem.CarPriceList.Add(new CarPrice()
                        {
                            Name = "2.0 HL 豪华型",
                            Price = "249,800"
                        });
                        break;
                    }
                #endregion
                #region NewElantra
                case "NewElantra":
                    {
                        carItem.CarPriceList.Add(new CarPrice()
                        {
                            Name = "1.6GS MT 手动时尚型",
                            Price = "105,800"
                        });
                        carItem.CarPriceList.Add(new CarPrice()
                        {
                            Name = "1.6GLS AT自动智能型",
                            Price = "115,800"
                        });
                        carItem.CarPriceList.Add(new CarPrice()
                        {
                            Name = "1.6DLX MT 手动尊贵型",
                            Price = "115,800"
                        });
                        carItem.CarPriceList.Add(new CarPrice()
                        {
                            Name = "1.6DLX AT自动尊贵型",
                            Price = "127,800"
                        });

                        break;
                    }
                #endregion
                #region lingdong
                case "lingdong":
                    {
                        carItem.CarPriceList.Add(new CarPrice()
                        {
                            Name = "1.6L 手动 智炫·青春型",
                            Price = "99,800"
                        });
                        carItem.CarPriceList.Add(new CarPrice()
                        {
                            Name = "1.6L 自动 智炫·青春型",
                            Price = "111,800"
                        });
                        carItem.CarPriceList.Add(new CarPrice()
                        {
                            Name = "1.6L 手动 智炫·活力型",
                            Price = "109,800"
                        });
                        carItem.CarPriceList.Add(new CarPrice()
                        {
                            Name = "1.6L 自动 智炫·精英型",
                            Price = "119,800"
                        });
                        carItem.CarPriceList.Add(new CarPrice()
                        {
                            Name = "1.6L 自动 智炫·豪华型",
                            Price = "133,800"
                        });
                        carItem.CarPriceList.Add(new CarPrice()
                        {
                            Name = "1.6L 自动 智炫·旗舰型",
                            Price = "145,800"
                        });
                        carItem.CarPriceList.Add(new CarPrice()
                        {
                            Name = "1.4T 双离合 炫动·活力型",
                            Price = "129,800"
                        });
                        carItem.CarPriceList.Add(new CarPrice()
                        {
                            Name = "1.4T 双离合 炫动·精英型",
                            Price = "137,800"
                        });
                        carItem.CarPriceList.Add(new CarPrice()
                        {
                            Name = "1.4T 双离合 炫动·旗舰型",
                            Price = "151,800"
                        });
                        break;
                    }
                #endregion
                #region Sonata9_Characteristic
                case "Sonata9_Characteristic":
                    {
                        carItem.CarPriceList.Add(new CarPrice()
                        {
                            Name = "2.0GLS 智能型",
                            Price = "174,800"
                        });
                        carItem.CarPriceList.Add(new CarPrice()
                        {
                            Name = "1.6TGDi GS 时尚型",
                            Price = "179,800"
                        });
                        carItem.CarPriceList.Add(new CarPrice()
                        {
                            Name = "1.6TGDi GX 舒适型",
                            Price = "186,800"
                        });
                        carItem.CarPriceList.Add(new CarPrice()
                        {
                            Name = "1.6TGDi GLS 智能型",
                            Price = "194,800"
                        });
                        carItem.CarPriceList.Add(new CarPrice()
                        {
                            Name = "1.6TGDi GLX 领先型",
                            Price = "207,800"
                        });
                        carItem.CarPriceList.Add(new CarPrice()
                        {
                            Name = "1.6TGDi DLX 尊贵型",
                            Price = "217,800"
                        });
                        carItem.CarPriceList.Add(new CarPrice()
                        {
                            Name = "2.4GDi  DLX 尊贵型",
                            Price = "217,800"
                        });
                        carItem.CarPriceList.Add(new CarPrice()
                        {
                            Name = "2.4GDi  LUX 至尊型",
                            Price = "227,800"
                        });
                        carItem.CarPriceList.Add(new CarPrice()
                        {
                            Name = "2.4GDi  TOP 旗舰型",
                            Price = "249,800"
                        });
                        break;
                    }
                #endregion
                #region IX25_Characteristic
                case "IX25_Characteristic":
                    {
                        carItem.CarPriceList.Add(new CarPrice()
                        {
                            Name = "1.6GS 2WD MT 手动时尚型",
                            Price = "119,800"
                        });
                        carItem.CarPriceList.Add(new CarPrice()
                        {
                            Name = "1.6GS 2WD AT 自动时尚型",
                            Price = "133,800"
                        });
                        carItem.CarPriceList.Add(new CarPrice()
                        {
                            Name = "1.6DLS 2WD AT 自动智能型",
                            Price = "142,800"
                        });
                        carItem.CarPriceList.Add(new CarPrice()
                        {
                            Name = "1.6DLX 2WD AT 自动尊贵型",
                            Price = "156,800"
                        });
                        carItem.CarPriceList.Add(new CarPrice()
                        {
                            Name = "2.0GLS AT 2WD 自动智能型",
                            Price = "166,800"
                        });
                        carItem.CarPriceList.Add(new CarPrice()
                        {
                            Name = "2.0GLX AT 4WD 自动四驱领先型",
                            Price = "179,800"
                        });
                        carItem.CarPriceList.Add(new CarPrice()
                        {
                            Name = "2.0DLX AT 4WD 自动四驱尊贵型",
                            Price = "186,800"
                        });
                        break;
                    }
                #endregion
                #region Santafe_Characteristic
                case "Santafe_Characteristic":
                    {
                        carItem.CarPriceList.Add(new CarPrice()
                        {
                            Name = "2.4 GLS 2WD MT 智能型",
                            Price = "224,800"
                        });
                        carItem.CarPriceList.Add(new CarPrice()
                        {
                            Name = "2.4 GLS 2WD AT 智能型",
                            Price = "239,800"
                        });
                        carItem.CarPriceList.Add(new CarPrice()
                        {
                            Name = "2.0T GLS 2WD AT 智能型7座",
                            Price = "239,800"
                        });
                        carItem.CarPriceList.Add(new CarPrice()
                        {
                            Name = "2.0T GLS 4WD AT 智能型7座",
                            Price = "249,800"
                        });
                        carItem.CarPriceList.Add(new CarPrice()
                        {
                            Name = "2.0T TOP 4WD AT 旗舰型7座",
                            Price = "289,800"
                        });
                        break;
                    }
                #endregion
                #region Langdong_Characteristic
                case "Langdong_Characteristic":
                    {
                        carItem.CarPriceList.Add(new CarPrice()
                        {
                            Name = "1.6 GS MT 手动时尚型",
                            Price = "105,800"
                        });
                        carItem.CarPriceList.Add(new CarPrice()
                        {
                            Name = "1.6 GS AT 自动时尚型",
                            Price = "115,800"
                        });
                        carItem.CarPriceList.Add(new CarPrice()
                        {
                            Name = "1.6 GLX MT 手动领先型",
                            Price = "115,800"
                        });
                        carItem.CarPriceList.Add(new CarPrice()
                        {
                            Name = "1.6 GLX AT 自动领先型",
                            Price = "127,800"
                        });
                        carItem.CarPriceList.Add(new CarPrice()
                        {
                            Name = "1.6 DLX AT 自动尊贵型",
                            Price = "134,800"
                        });
                        carItem.CarPriceList.Add(new CarPrice()
                        {
                            Name = "1.8 DLX MT 手动尊贵型",
                            Price = "139,800"
                        });
                        carItem.CarPriceList.Add(new CarPrice()
                        {
                            Name = "1.8 DLX AT 自动尊贵型",
                            Price = "149,800"
                        });
                        break;
                    }
                #endregion
                #region Mistra
                case "Mistra":
                    {
                        carItem.CarPriceList.Add(new CarPrice()
                        {
                            Name = "1.8 GL MT 手动舒适型",
                            Price = "129,800"
                        });
                        carItem.CarPriceList.Add(new CarPrice()
                        {
                            Name = "1.8 GL AT 自动舒适型",
                            Price = "139,800"
                        });
                        carItem.CarPriceList.Add(new CarPrice()
                        {
                            Name = "1.8 GLS AT 自动智能型",
                            Price = "149,800"
                        });
                        carItem.CarPriceList.Add(new CarPrice()
                        {
                            Name = "1.8 DLX AT 自动尊贵型",
                            Price = "159,800"
                        });
                        carItem.CarPriceList.Add(new CarPrice()
                        {
                            Name = "2.0 LUX AT 自动尊享型",
                            Price = "176,800"
                        });
                        carItem.CarPriceList.Add(new CarPrice()
                        {
                            Name = "1.6T-GDi GLS AT 自动智能型",
                            Price = "159,800"
                        });
                        carItem.CarPriceList.Add(new CarPrice()
                        {
                            Name = "1.6T-Gdi TOP AT 自动旗舰型",
                            Price = "169,800"
                        });
                        break;
                    }
                #endregion
                #region Verna_Characteristic
                case "Verna_Characteristic":
                    {
                        carItem.CarPriceList.Add(new CarPrice()
                        {
                            Name = "1.4 GS MT 手动时尚型",
                            Price = "73,900"
                        });
                        carItem.CarPriceList.Add(new CarPrice()
                        {
                            Name = "1.4 GS AT 自动时尚型",
                            Price = "81,900"
                        });
                        carItem.CarPriceList.Add(new CarPrice()
                        {
                            Name = "1.4 GLS MT 手动智能型",
                            Price = "78,900"
                        });
                        carItem.CarPriceList.Add(new CarPrice()
                        {
                            Name = "1.4 GLS AT 自动智能型",
                            Price = "86,900"
                        });
                        carItem.CarPriceList.Add(new CarPrice()
                        {
                            Name = "1.4 GLX AT 自动领先型",
                            Price = "92,900"
                        });
                        carItem.CarPriceList.Add(new CarPrice()
                        {
                            Name = "1.4 TOP MT 手动顶级型",
                            Price = "88,900"
                        });
                        carItem.CarPriceList.Add(new CarPrice()
                        {
                            Name = "1.4 TOP AT 自动顶级型",
                            Price = "102,900"
                        });
                        carItem.CarPriceList.Add(new CarPrice()
                        {
                            Name = "1.6 GLX AT 自动领先型",
                            Price = "99,900"
                        });
                        carItem.CarPriceList.Add(new CarPrice()
                        {
                            Name = "1.6 TOP AT 自动顶级型",
                            Price = "106,900"
                        });
                        break;
                    }
                #endregion
                #region Ix35_Characteristic
                case "Ix35_Characteristic":
                    {

                        carItem.CarPriceList.Add(new CarPrice()
                        {
                            Name = "2.0 GL 2WD MT 舒适型",
                            Price = "149,800"
                        });
                        carItem.CarPriceList.Add(new CarPrice()
                        {
                            Name = "2.0 GL 2WD AT 舒适型",
                            Price = "163,800"
                        });
                        carItem.CarPriceList.Add(new CarPrice()
                        {
                            Name = "2.0 GLS 2WD AT 智能型",
                            Price = "176,800"
                        });
                        carItem.CarPriceList.Add(new CarPrice()
                        {
                            Name = "2.0 GLS 4WD AT 智能型",
                            Price = "201,800"
                        });
                        carItem.CarPriceList.Add(new CarPrice()
                        {
                            Name = "2.0 GLX 2WD AT 领先型",
                            Price = "195,800"
                        });
                        carItem.CarPriceList.Add(new CarPrice()
                        {
                            Name = "2.4 GLX 2WD AT 领先型",
                            Price = "196,800"
                        });
                        carItem.CarPriceList.Add(new CarPrice()
                        {
                            Name = "2.4 GLX 4WD AT 领先型",
                            Price = "222,800"
                        });
                        break;
                    }
                #endregion
                #region Tucson_Characteristic
                case "Tucson_Characteristic":
                    {
                        carItem.CarPriceList.Add(new CarPrice()
                        {
                            Name = "2.4 GLX 4WD AT 领先型",
                            Price = "222,800"
                        });
                        break;
                    }
                #endregion
                #region Elantrayuedong_Characteristic
                case "Elantrayuedong_Characteristic":
                    {
                        break;
                    }
                #endregion
                #region Sonata8_Characteristic
                case "Sonata8_Characteristic":
                    {
                        break;
                    }
                #endregion
                default: break;
            }
            return carItem;
        }
    }
}