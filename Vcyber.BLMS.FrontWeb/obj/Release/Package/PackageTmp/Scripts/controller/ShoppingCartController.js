var ShoppingCart = {
    name: "mycart",
    cart: {
        totalQuantity: 0,
        totalPrice: 0,
        totalProductPrice: 0,
        totalBlueBeanQuantity: 0,
        totalBlueBean: 0,
        totalIntegralQuantity: 0,
        totalIntegral: 0,
        productList: [
            //{ productId: "a",skuId:(productId_payType)a_1, productName: "a", price: "10", quantity: "2",imgUrl:"",blueBean:"",Integral:"",payType:"1" },
            //{ productId: "a",skuId:(productId_payType)a_2, productName: "a", price: "20", quantity: "5",imgUrl:"",blueBean:"",Integral:"",payType:"2" },
            //{ productId: "",skuId:(productId_payType), productName: "", price: "", quantity: "",imgUrl:"",blueBean:"",Integral:"",payType:""}
        ]
    }
};


//添加商品
ShoppingCart.AddItem = function (item) {
    //判断购物车中是否已经存在该商品
    var isHave = false;
    var isStockOver = false;
    var stockValue = parseFloat(item.quantity);
    //先判断cookie中是否存在购物车
    var mycart = this.MyCart();




    if (typeof mycart != "undefined" && mycart !== null && mycart !== "" && mycart.totalQuantity > 0) {
        this.cart = mycart;
    }
    //添加商品
    ShoppingCart.cart.totalQuantity = parseInt(ShoppingCart.cart.totalQuantity) + parseInt(item.quantity);
    ShoppingCart.cart.totalPrice = parseFloat(ShoppingCart.cart.totalPrice) + parseFloat(item.quantity) * parseFloat(item.price);
    ShoppingCart.cart.totalProductPrice = parseFloat(ShoppingCart.cart.totalProductPrice) + parseFloat(item.quantity) * parseFloat(item.price);


    if (item.payType == "BlueBean") {
        ShoppingCart.cart.totalBlueBeanQuantity = parseInt(ShoppingCart.cart.totalBlueBeanQuantity) + parseInt(item.quantity);
        ShoppingCart.cart.totalBlueBean = parseFloat(ShoppingCart.cart.totalBlueBean) + parseFloat(item.price) * parseInt(item.quantity);
    } else {
        ShoppingCart.cart.totalIntegralQuantity = parseInt(ShoppingCart.cart.totalIntegralQuantity) + parseInt(item.quantity);
        ShoppingCart.cart.totalIntegral = parseFloat(ShoppingCart.cart.totalIntegral) + parseFloat(item.price) * parseInt(item.quantity);
    }

    //以下forEach在IE6，7，8中不支持
    //ShoppingCart.cart.productList.forEach(function (obj) {
    //    if (obj.skuId == item.skuId) {
    //        obj.quantity = parseFloat(item.quantity) + parseFloat(obj.quantity);
    //        isHave = true;
    //        return false;
    //    }
    //});
    for (var i = 0, len = ShoppingCart.cart.productList.length; i < len; i++) {
        var obj = ShoppingCart.cart.productList[i];

        //if (obj.skuId == item.skuId)
        //{
        //    obj.quantity = parseFloat(item.quantity) + parseFloat(obj.quantity);
        //    stockValue = obj.quantity;
        //}
        if (obj.skuId == item.skuId) {
           
            stockValue +=parseFloat(obj.quantity);
            if (obj.producttype == item.producttype && obj.productcolor == item.productcolor) {
                obj.quantity = parseFloat(item.quantity) + parseFloat(obj.quantity);
                isHave = true;
                break;
            }
           
           
           
        }
    }


   
    if (stockValue > parseInt($("#productStock").text())) {
        popWindownBlue( "不能超过库存数量");
        return false;
    }
    if (!isHave) {
        ShoppingCart.cart.productList.push(item);
    }
    ShoppingCart.SaveCart();
};


//删除商品
ShoppingCart.RemoveItem = function (item) {
    var index = 0;
    //将cookie中的购物车赋值给当前购物车
    ShoppingCart.cart = ShoppingCart.MyCart();
    ShoppingCart.cart.productList.forEach(function (obj) {
        if (obj.skuId == item.skuId) {
            //如果删除的skuid存在购物车中，则将其删除
            ShoppingCart.cart.totalQuantity = parseInt(ShoppingCart.cart.totalQuantity) - parseInt(obj.quantity);
            ShoppingCart.cart.totalPrice = parseInt(ShoppingCart.cart.totalPrice) - parseInt(obj.price) * parseInt(obj.quantity);
            if (obj.payType == "Integral") {
                ShoppingCart.cart.totalIntegralQuantity = parseInt(ShoppingCart.cart.totalIntegralQuantity) - parseInt(obj.quantity);
                ShoppingCart.cart.totalIntegral = parseInt(ShoppingCart.cart.totalIntegral) - parseInt(obj.price) * parseInt(obj.quantity);
            } else {
                ShoppingCart.cart.totalBlueBeanQuantity = parseInt(ShoppingCart.cart.totalBlueBeanQuantity) - parseInt(obj.quantity);
                ShoppingCart.cart.totalBlueBean = parseInt(ShoppingCart.cart.totalBlueBean) - parseInt(obj.price) * parseInt(obj.quantity);
            }
            ShoppingCart.cart.productList.splice(index, 1);
            return false;
        }
        index++;
    });

    if (ShoppingCart.cart.productList.length <= 0) {
        ShoppingCart.RemoveAll();
    }
    //删除成功后保存购物车
    ShoppingCart.SaveCart();
};


//清空购物车
ShoppingCart.RemoveAll = function () {
    ShoppingCart.cart.totalQuantity = 0;
    ShoppingCart.cart.totalPrice = 0;
    ShoppingCart.cart.totalProductPrice = 0;
    ShoppingCart.cart.totalBlueBeanQuantity = 0;
    ShoppingCart.cart.totalBlueBean = 0;
    ShoppingCart.cart.totalIntegralQuantity = 0;
    ShoppingCart.cart.totalIntegral = 0;
    ShoppingCart.cart.productList = [];
    ShoppingCart.SaveCart();
};


//我的购物车
ShoppingCart.MyCart = function () {
    var cookieHelper = new Cookie();
    var mycartobj = cookieHelper.getCookie(this.name);
    if (typeof mycartobj == "undefined" || mycartobj === null || mycartobj === "") {
        return ShoppingCart.cart;
    }
    return JSON.parse(mycartobj);

};


//保存购物车
ShoppingCart.SaveCart = function () {
    var cookieHelper = new Cookie();
    cookieHelper.setCookie(this.name, JSON.stringify(ShoppingCart.cart), 1);
    $("#productdetailmycartquantity").html(this.ProductQuantity());
    $("#CartByMallTotalQuantity").html(this.ProductQuantity());
};


//购物车中商品的数量
ShoppingCart.ProductQuantity = function () {
    //return ShoppingCart.MyCart().productList.length;
    return ShoppingCart.MyCart().totalQuantity;
};


//购物车页面数据统计
ShoppingCart.CartStatistics = function () {
    var _totalQuantity = 0;
    var _totalBlueBeanQuantity = 0;
    var _totalBlueBeanPrice = 0;
    var _totalIntegralQuantity = 0;
    var _totalIntegralPrice = 0;


    $(".mycartProductItem").each(function (i, obj) {
        var checkedItem = $(obj).find("input[name='mycartProductCheckItem']");
        if (!checkedItem.is(":checked")) {
            return;
        }
        var _productId = $(obj).find("input[id^='mycartItemProductQuantity']").val();
        var _skuId = $(obj).find("input[id^='mycartItemProductSkuId']").val();
        var _productName = $(obj).find("label[id^='mycartItemProductSkuName']").html();
        var _totalprice = $(obj).find("label[id^='mycartItemProductTotalPrice']").html();
        var _price = $(obj).find("label[id^='mycartItemProductPrice']").html();
        var _quantity = $(obj).find("input[id^='mycartItemProductQuantity']").val();
        var _imgUrl = "";
        var _payType = $(obj).find("label[id^='mycartItemProductType']").attr("title");

        _totalQuantity = parseInt(_totalQuantity) + parseInt(_quantity);
        if (_payType == "Integral") {
            _totalIntegralPrice = parseFloat(_totalIntegralPrice) + parseFloat(_price) * parseFloat(_quantity);
        } else {
            _totalBlueBeanPrice = parseFloat(_totalBlueBeanPrice) + parseFloat(_price) * parseFloat(_quantity);
        }

    });
    $("#cartProductCheckQuantity").html(_totalQuantity);
    $("#mycartProductScoreSum").html(_totalIntegralPrice);
    $("#mycartProductBlueBeanSum").html(_totalBlueBeanPrice);
};























//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

/** 
Cookie类 
*/
function Cookie() {
    /** 
  　　@desc 设置Cookie 
  　　@return void 
  　　*/
    this.setCookie = function (name, value, hours) {
        var expire = "";
        if (hours !== null) {
            expire = new Date((new Date()).getTime() + hours * 3600000);
            expire = "; expires=" + expire.toGMTString();
        }
        var path = ";path=/";
        document.cookie = escape(name) + "=" + escape(value) + expire + path;
    };

    /** 
  　　@desc 读取Cookie 
  　　@return String 
  　　*/
    this.getCookie = function (name) {
        var cookieValue = "";
        var search = escape(name) + "=";
        if (document.cookie.length > 0) {
            offset = document.cookie.indexOf(search);
            if (offset != -1) {
                offset += search.length;
                end = document.cookie.indexOf(";", offset);
                if (end == -1) end = document.cookie.length;
                cookieValue = unescape(document.cookie.substring(offset, end));
            }
        }
        return cookieValue;
    };
}


function CarCart(name) {

    if (!window.clientInformation.cookieEnabled) {
        popWindownBlue( '你的浏览器不支持Cookie无法使用此 购物车 系统');
        return false;
    }

    //##内部变量############################################################# 

    this.carName = name;
    this.expire = 24 * 30;　　　　//购物车的有效时间(30天) 
    this.carDatas = new Array();
    this.cookie = new Cookie();

    //##内部对象############################################################# 

    this.typeObj = function (name, value) {　　//自带的 类别 对象 
        this.name = name;
        this.value = "value";
    };
    this.proObj = function (name, value) {　　//自带的" 商品 对象 
        this.name = name;
        this.value = value;
    };

    //##私有方法列表########################################################## 
    // 
    //　　getTypePoint(typeName);　　　　　　　　//得到购物车里类别数组里的下标 
    //　　getProPoint(typeName,proName);　　　　//得到购物车里类别下的产品下标 
    //　　saveCookie()　　　　　　　　　　　　//以特定的形式存储此购物车的Cookie 
    // 
    //######################################################################## 

    /** 
  　　@desc 得到购物车里类别数组里的下标，找到的话返回下标，否则返回 -1 
  　　@return int 
  　　*/
    this.getTypePoint = function (typeName) {
        var isok = false;
        var i = 0;
        for (; i < this.carDatas.length; i++) {
            if (this.carDatas[i].name == typeName) {
                isok = true;　　　　　　//找到位置 
                break;
            }
        }
        if (isok) return i;
        else return -1;
    };

    /** 
  　　@desc 得到购物车里类别下的产品下标，找到返回下标，否则返回 -1 
  　　@return int 
  　　*/
    this.getProPoint = function (typeId, proName) {
        var isok = false;
        var j = 0;
        var tempProObj = this.carDatas[typeId].value;
        for (; j < tempProObj.length; j++) {
            if (tempProObj[j].name == proName) {
                isok = true;
                break;
            }
        }
        if (isok) return j;
        else return -1;
    };

    /** 
  　　@desc 存储生成的Cookie字符串 
  　　@return void 
  　　*/
    this.saveCookie = function () {
        var outStr = '';
        for (i = 0; i < this.carDatas.length; i++) {
            var typeName = this.carDatas[i].name;
            var typeValue = this.carDatas[i].value;
            var proOutStr = '';
            for (j = 0; j < typeValue.length; j++) {
                if (j === 0) {
                    proOutStr = typeValue[j].name + ':' + typeValue[j].value;
                }
                else {
                    proOutStr += '|' + typeValue[j].name + ':' + typeValue[j].value;
                }
            }
            if (i === 0) {
                outStr = typeName + '#' + proOutStr;
            }
            else {
                outStr += ',' + typeName + '#' + proOutStr;
            }
        }
        this.cookie.setCookie(this.carName, outStr, this.expire);　　//存入 Cookie　　 
    };

    //##构造语句############################################################ 

    if (this.cookie.getCookie(name) === '') {
        this.cookie.setCookie(name, '', this.expire);
    } else {
        var tempTypes = this.cookie.getCookie(name).split(',');
        for (i = 0; i < tempTypes.length; i++) {
            var tempTypeObj = tempTypes[i].split('#');
            var type_pro = new Array();
            if (tempTypeObj[1]) {
                var tempProObj = tempTypeObj[1].split('|');
                for (j = 0; j < tempProObj.length; j++) {
                    var proDesc = tempProObj[j].split(':');
                    type_pro.push(new this.proObj(proDesc[0], proDesc[1]));
                }
            }
            this.carDatas.push(new this.typeObj(tempTypeObj[0], type_pro));
        }
    }

    //##公共方法列表######################################################### 
    // 
    //　　addType(typeName);　　　　　　　　　　//增加一个类别 
    //　　addPro(typeName,proName,value);　　　　//增加一个产品 
    //　　editPro(typeName,proName,value);　　//修改产品的值 
    //　　delPro(typeName,proName);　　　　　　//删除购物车内的一个类别下的产品 
    //　　delType(typeName);　　　　　　　　　　//删除购物车内的一个类别，包括类别下的产品 
    //　　delCar();　　　　　　　　　　　　　　//删除购物车 
    //　　 
    //　　getCar();　　　　　　　　　　　　　　//得到整个购物车的数据 
    //　　getType();　　　　　　　　　　　　　　//得到购物车内的所有类别列表 
    //　　getPro(typeName);　　　　　　　　　　//得到购物车内指定类别下的产品列表 
    //　　getProVal(typeName,proName);　　　　//得到购物车内指定类别下的产品属性 
    // 
    //######################################################################## 

    /** 
  　　@desc 在购物车里增加一个类别，增加成功返回真，否则返回假 
  　　@return bool 
  　　*/
    this.addType = function (typeName) {
        if (this.getTypePoint(typeName) != -1) return false;　　　　　　　　//如果已经有此类别了，返回假 
        this.carDatas.push(new this.typeObj(typeName, new Array()));　　　　　　//push进 自身数组 
        this.saveCookie();　　//存入 Cookie 
        return true;
    };

    /** 
  　　@desc 在购物车里增加一个产品，增加成功返回真，否则返回假 
  　　@return bool 
  　　*/
    this.addPro = function (typeName, proName, value) {
        var typePoint = this.getTypePoint(typeName); if (typePoint == -1) return false;　　　　//没有此类别，无法增加，返回假 
        var proPoint = this.getProPoint(typePoint, proName); if (proPoint != -1) return false;　　　　//有此产品了，无法增加重复，返回假 
        this.carDatas[typePoint].value.push(new this.proObj(proName, value));　　//push到自身数组 
        this.saveCookie();　　//存入 Cookie 
        return true;
    };

    /** 
  　　@desc 修改购物车里的产品属性 
  　　@return bool 
  　　*/
    this.editPro = function (typeName, proName, value) {
        var typePoint = this.getTypePoint(typeName); if (typePoint == -1) return false;　　//没有此类别，无法修改，返回假 
        var proPoint = this.getProPoint(typePoint, proName); if (proPoint == -1) return false;　　//没有此产品，无法修改，返回假 
        this.carDatas[typePoint].value[proPoint].value = value;　　　　　　　　　　　　　　//更新自身  
        this.saveCookie();　　//存入 Cookie 
        return true;
    };

    /** 
  　　@desc 删除一个产品 
  　　@return bool 
  　　*/
    this.delPro = function (typeName, proName) {
        var typePoint = this.getTypePoint(typeName); if (typePoint == -1) return false;　　//没有此类别，无法删除，返回假 
        var proPoint = this.getProPoint(typePoint, proName); if (proPoint == -1) return false;　　//没有此产品，无法删除，返回假 
        var pros = this.carDatas[typePoint].value.length;
        this.carDatas[typePoint].value[proPoint] = this.carDatas[typePoint].value[pros - 1];　　//最后一个产品放置要删除的产品上 
        this.carDatas[typePoint].value.pop();
        this.saveCookie();　　//存入 Cookie 
        return true;
    };

    /** 
  　　@desc 删除一个类别 
  　　@return bool 
  　　*/
    this.delType = function (typeName) {
        var typePoint = this.getTypePoint(typeName); if (typePoint == -1) return false;　　//没有此类别，无法删除，返回假 
        var types = this.carDatas.length;
        this.carDatas[typePoint] = this.carDatas[types - 1];　　　　　　　　　　　　//删除类别 
        this.carDatas.pop();
        this.saveCookie();　　//存入 Cookie 
        return true;
    };

    /** 
  　　@desc 删除此购物车 
  　　@return void 
  　　*/
    this.delCar = function () {
        this.cookie.setCookie(this.carName, '', 0);
        this.carDatas = new Array();
        this.saveCookie();　　//存入 Cookie 
    };

    /** 
  　　@desc 获得购物车数据 
  　　@return Array 
  　　*/
    this.getCar = function () {
        return this.carDatas;
    };

    /** 
  　　@desc 获得类别列表 
  　　@return Array 
  　　*/
    this.getType = function () {
        var returnarr = new Array();
        for (i = 0; i < this.carDatas.length; i++) returnarr.push(this.carDatas[i].name);
        return returnarr;
    };

    /** 
  　　@desc 获得类别下的产品列表 
  　　@return Array 
  　　*/
    this.getPro = function (typeName) {
        var typePoint = this.getTypePoint(typeName); if (typePoint == -1) return false;　　//没有此类别，返回假 
        return this.carDatas[typePoint].value;
    };

    /** 
  　　@desc 获得商品属性 
  　　@return String 
  　　*/
    this.getProVal = function (typeName, proName) {
        var typePoint = this.getTypePoint(typeName); if (typePoint == -1) return false;　　//没有此类别，返回假 
        var proPoint = this.getProPoint(typePoint, proName); if (proPoint == -1) return false;　　//没有此产品，返回假 
        return this.carDatas[typePoint].value[proPoint].value;
    };
}