/********************************************************************************************************
* Description: javascript系统函数扩展方法
* Auther: ZhangPan
* Create Date: 2012-07-31
*********************************************************************************************************/
//删除左侧指定字符
String.prototype.lTrim = function (s) {
    s = (s ? s : "\\s");
    s = ("(" + s + ")");
    var reg_lTrim = new RegExp("^" + s + "*", "g");
    return this.replace(reg_lTrim, "");
};

//删除右侧指定字符
String.prototype.rTrim = function (s) {
    s = (s ? s : "\\s");
    s = ("(" + s + ")");
    var reg_rTrim = new RegExp(s + "*$", "g");
    return this.replace(reg_rTrim, "");
};

//删除左侧和右侧的指定字符
String.prototype.trim = function (s) {
    s = (s ? s : "\\s");
    s = ("(" + s + ")");
    var reg_trim = new RegExp("(^" + s + "*)|(" + s + "*$)", "g");
    return this.replace(reg_trim, "");
};

//格式化JSON格式字符串为日期格式
String.prototype.FormatJsonDate = function (format) {
    if (this.toString() === "/Date(-62135596800000+0800)/") { return ""; }
    var cellval = this;
    if (cellval == null || cellval == "" || cellval == undefined) { return ""; }
    var date = new Date(parseInt(cellval.replace("/Date(", "").replace(")/", ""), 10));
    var month = date.getMonth() + 1 < 10 ? "0" + (date.getMonth() + 1) : date.getMonth() + 1;
    var currentDate = date.getDate() < 10 ? "0" + date.getDate() : date.getDate();
    if (format) {
        return date.Format(format);
    } else {
        return date.getFullYear() + "-" + month + "-" + currentDate;
    }
}

//扩展Date对象
Date.prototype.Format = function (format) {
    var obj =
     {
         "M+": this.getMonth() + 1, //返回实际月份   
         "d+": this.getDate(), //返回当月第几天   
         "H+": this.getHours(), //返回小时   
         "m+": this.getMinutes(), //返回分钟   
         "s+": this.getSeconds(), //返回秒   
         "q+": Math.floor((this.getMonth() + 3) / 3), //返回第几个季度   
         "S": this.getMilliseconds(), //返回毫秒   
         "w": this.getDay(), //返回星期几，0为星期日   
         "W": "日一二三四五六".charAt(this.getDay()) //返回星期几的中文表示   
     }

    // 年的单独处理   
    if (/(y+)/.test(format))
        format = format.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    // 其它格式处理   
    for (var k in obj) {
        if (new RegExp("(" + k + ")").test(format))
            format = format.replace(RegExp.$1, RegExp.$1.length == 1 ? obj[k] : ("00" + obj[k]).substr(("" + obj[k]).length));
    }
    return format;
}

//日期计算
Date.prototype.DateAdd = function (strInterval, Number) {
    var dtTmp = this;
    switch (strInterval) {
        case 's': return new Date(Date.parse(dtTmp) + (1000 * Number));
        case 'n': return new Date(Date.parse(dtTmp) + (60000 * Number));
        case 'h': return new Date(Date.parse(dtTmp) + (3600000 * Number));
        case 'd': return new Date(Date.parse(dtTmp) + (86400000 * Number));
        case 'w': return new Date(Date.parse(dtTmp) + ((86400000 * 7) * Number));
        case 'q': return new Date(dtTmp.getFullYear(), (dtTmp.getMonth()) + Number * 3, dtTmp.getDate(), dtTmp.getHours(), dtTmp.getMinutes(), dtTmp.getSeconds());
        case 'm': return new Date(dtTmp.getFullYear(), (dtTmp.getMonth()) + Number, dtTmp.getDate(), dtTmp.getHours(), dtTmp.getMinutes(), dtTmp.getSeconds());
        case 'y': return new Date((dtTmp.getFullYear() + Number), dtTmp.getMonth(), dtTmp.getDate(), dtTmp.getHours(), dtTmp.getMinutes(), dtTmp.getSeconds());
    }
}

//比较日期差 dtEnd 格式为日期型或者 有效日期格式字符串
Date.prototype.DateDiff = function (strInterval, dtEnd) {
    var dtStart = this;
    if (typeof dtEnd == 'string')//如果是字符串转换为日期型 
    {
        dtEnd = StringToDate(dtEnd);
    }
    switch (strInterval) {
        case 's': return parseInt((dtEnd - dtStart) / 1000);
        case 'n': return parseInt((dtEnd - dtStart) / 60000);
        case 'h': return parseInt((dtEnd - dtStart) / 3600000);
        case 'd': return parseInt((dtEnd - dtStart) / 86400000);
        case 'w': return parseInt((dtEnd - dtStart) / (86400000 * 7));
        case 'm': return (dtEnd.getMonth() + 1) + ((dtEnd.getFullYear() - dtStart.getFullYear()) * 12) - (dtStart.getMonth() + 1);
        case 'y': return dtEnd.getFullYear() - dtStart.getFullYear();
    }
}

//日期输出字符串，重载了系统的toString方法 
Date.prototype.toString = function (showWeek) {
    var myDate = this;
    var str = myDate.toLocaleDateString();
    if (showWeek) {
        var Week = ['日', '一', '二', '三', '四', '五', '六'];
        str += ' 星期' + Week[myDate.getDay()];
    }
    return str;
}

//把日期分割成数组
Date.prototype.toArray = function () {
    var myDate = this;
    var myArray = Array();
    myArray[0] = myDate.getFullYear();
    myArray[1] = myDate.getMonth();
    myArray[2] = myDate.getDate();
    myArray[3] = myDate.getHours();
    myArray[4] = myDate.getMinutes();
    myArray[5] = myDate.getSeconds();
    return myArray;
}

//+--------------------------------------------------- 
//| 取得日期数据信息 
//| 参数 interval 表示数据类型 
//| y 年 m月 d日 w星期 ww周 h时 n分 s秒 
//+--------------------------------------------------- 
Date.prototype.DatePart = function (interval) {
    var myDate = this;
    var partStr = '';
    var Week = ['日', '一', '二', '三', '四', '五', '六'];
    switch (interval) {
        case 'y': partStr = myDate.getFullYear(); break;
        case 'm': partStr = myDate.getMonth() + 1; break;
        case 'd': partStr = myDate.getDate(); break;
        case 'w': partStr = Week[myDate.getDay()]; break;
        case 'ww': partStr = myDate.WeekNumOfYear(); break;
        case 'h': partStr = myDate.getHours(); break;
        case 'n': partStr = myDate.getMinutes(); break;
        case 's': partStr = myDate.getSeconds(); break;
    }
    return partStr;
}

//取得当前日期所在月的最大天数
Date.prototype.MaxDayOfDate = function () {
    var myDate = this;
    var ary = myDate.toArray();
    var date1 = (new Date(ary[0], ary[1] + 1, 1));
    var date2 = date1.dateAdd(1, 'm', 1);
    var result = dateDiff(date1.Format('yyyy-MM-dd'), date2.Format('yyyy-MM-dd'));
    return result;
}

//取得当前日期所在周是一年中的第几周
Date.prototype.WeekNumOfYear = function () {
    var myDate = this;
    var ary = myDate.toArray();
    var year = ary[0];
    var month = ary[1] + 1;
    var day = ary[2];
    document.write('< script language=VBScript\> \n');
    document.write('myDate = DateValue(' + month + '-' + day + '-' + year + ') \n');
    document.write("result = DatePart('ww', myDate) \n");
    document.write(' \n');
    return result;
}



/********************************************************************************************************
* Description: 公共方法
* Add Author: ZhangPan
* Create Date: 2012-08-21 11:59:37
*********************************************************************************************************/
var common = function () { };
(function (commonObj) {
    commonObj.Args = {};
    commonObj.settings = {
        virtualPath: "/",   //虚拟路径
        virtualUrl: "/" //虚拟Url
    };
    function getThisArgs(src) {
        src = src.replace(/%2f/g, "/");
        if (src.indexOf("?") == -1) { return; }
        var paramsArr = src.split('?')[1].split('&');
        var args = {}, argsStr = [], param, t, name, value;
        for (var i = 0, len = paramsArr.length; i < len; i++) {
            param = paramsArr[i].split('=');
            name = param[0], value = param[1];
            if (typeof args[name] == "undefined") { //参数尚不存在
                args[name] = value;
            }
        }

        commonObj.Args = args;
        if (commonObj.Args && commonObj.Args.virtualpath) {
            var path = commonObj.Args.virtualpath.toString().trim("/");
            if (path != "") {
                commonObj.settings.virtualPath = "/" + path;
            }
        }
        if (commonObj.Args && commonObj.Args.virtualurl) {
            var url = commonObj.Args.virtualurl.toString().trim("/");
            if (url != "") {
                commonObj.settings.virtualUrl = "/" + url;
            }
        }
    }
    //commonObj.rootUrl 网站根url
    commonObj.rootUrl = (function (script, i, me) {
        var l = script.length;
        for (i = 0; i < l; i++) {
            me = !!document.querySelector ? script[i].src.toLowerCase() : script[i].getAttribute('src', 4).toLowerCase();
            if (me.substr(me.lastIndexOf('/')).indexOf("common.method") !== -1) {
                getThisArgs(me);
                break;
            }
        }
        me = me.split('?');
        _args = me[1];
        //return me[0].substr(0, me[0].lastIndexOf(commonObj.settings.virtualPath + "js/") + 1);
        //return me[0].substr(0, me[0].lastIndexOf("/js/") + 1);
        //return me[0].substr(0, me[0].lastIndexOf(commonObj.settings.virtualPath + "/js/") + 1);
        return me[0].substr(0, me[0].lastIndexOf("/contents/scripts/me/common.method.js"));
    })(document.getElementsByTagName('script'), 0);

    //用于注册与执行页面加载完毕后执行的方法
    commonObj.page_load = {
        jsLoadList: new Array(),
        exec: function () {
            // $(function(){
            if (commonObj.page_load.jsLoadList.length > 0) {
                for (var i = 0; i < commonObj.page_load.jsLoadList.length; i++) {
                    if (commonObj.page_load.jsLoadList[i]) {
                        commonObj.page_load.jsLoadList[i]();
                    }
                }
            }
            if (commonObj.page_load.jsLoadList.length > 50) {
                alert("请检查common.reg_jsLoad方法的queue参数，一般情况下load方法数不会大于50，如果确定存在这种情况，请更改此判断");
            }
            //   });
        },
        reg: function (fun, queue) {
            if (queue) {
                if (queue > commonObj.page_load.jsLoadList.length) {
                    for (var i = commonObj.page_load.jsLoadList.length; i < queue; i++) {
                        commonObj.page_load.jsLoadList[i] == undefined;
                    }
                }
                if (commonObj.page_load.jsLoadList[queue]) {
                    for (var i = commonObj.page_load.jsLoadList.length; i > queue; i--) {
                        commonObj.page_load.jsLoadList[i] = commonObj.page_load.jsLoadList[i - 1];
                    }
                }
                commonObj.page_load.jsLoadList[queue] = fun;
            } else {
                commonObj.page_load.jsLoadList[commonObj.page_load.jsLoadList.length] = fun;
            }
        }
    }

    //解决url
    commonObj.resolveUrl = function (url, isRoot, isFilePath) {
        if (!url) { return url; }
        if (url.indexOf("://") != -1) { return url; }
        //var reg_trim = new RegExp("(^(/)*)|((/)*$)", "g");
        if (!isRoot) {
            if (isFilePath) {
                var tempPath = commonObj.settings.virtualPath.trim("/");
                tempPath = tempPath ? "/" + tempPath + "/" : "/";
                return commonObj.rootUrl.trim("/") + tempPath + url.lTrim("/");
            } else {
                var tempUrl = commonObj.settings.virtualUrl.trim("/");
                tempUrl = tempUrl ? "/" + tempUrl + "/" : "/";
                return commonObj.rootUrl.trim("/") + tempUrl + url.lTrim("/");
            }
        } else {
            if (isFilePath) {
                return commonObj.rootUrl.trim("/") + "/" + url.lTrim("/");
            } else {
                return commonObj.rootUrl.trim("/") + "/" + url.lTrim("/");
            }
        }
    }

    //调用api
    commonObj.api = function(source, params, isRoot) {
        if (!source) {
            return source;
        }
        var url;
        if (source.indexOf("://") != -1) {
            return source;
        }
        if (!isRoot) {
            var tempUrl = commonObj.settings.virtualUrl.trim("/");
            tempUrl = tempUrl ? "/" + tempUrl + "/" : "/";
            url = commonObj.rootUrl.trim("/") + tempUrl + source.lTrim("/");
            if (params) {
                url += "?" + $.param(params);
            }
        } else {
            url = commonObj.rootUrl.trim("/") + "/" + source.lTrim("/");
            if (params) {
                url += "?" + $.param(params);
            }
        }
        if (url.indexOf("weixin/weixin") >= 0) url.replace("weixin/weixin", "weixin");
        return url;
    };

    //调用页面
    commonObj.view = function (source, params, isRoot) {
        if (!source) { return source; }
        if (!isRoot) {
            var tempUrl = commonObj.settings.virtualUrl.trim("/");
            tempUrl = tempUrl ? "/" + tempUrl + "/" : "/";
            var url = commonObj.rootUrl.trim("/") + tempUrl + source.lTrim("/");
            if (params) { url += "?" + $.param(params); }
            return url;
        } else {
            var url = commonObj.rootUrl.trim("/") + "/" + source.lTrim("/");
            if (params) { url += "?" + $.param(params); }
            return url;
        }
    }

    //引用文件
    commonObj.using = function (path, isRoot) {
        /// <summary>引用js或css文件</summary>
        /// <param name="path" type="string">文件虚拟路径</param>
        path = path.toLowerCase();
        var ext = path.substring(path.lastIndexOf("."));
        if (ext == ".js") {
            var js = document.getElementsByTagName("script");
            var isExists = false;
            for (var i = 0; i < js.length; i++) {
                var src = !!document.querySelector ?
		            js[i].src : js[i].getAttribute('src', 4);
                src = src.toLowerCase();
                if (src.indexOf(path) != -1) {
                    isExists = true;
                    break;
                }
            }
            if (!isExists) {
                document.write("<script type=\"text/javascript\" src=\"" + commonObj.resolveUrl(path, isRoot, true) + "\"></script>");
            }
        } else if (ext == ".css") {
            var css = document.getElementsByTagName("link");
            var isExists = false;
            for (var i = 0; i < css.length; i++) {
                var src = !!document.querySelector ?
		            css[i].href : css[i].getAttribute('href', 4);
                src = src.toLowerCase();
                if (src.indexOf(path) != -1) {
                    isExists = true;
                    break;
                }
            }
            if (!isExists) {
                document.write("<link href=\"" + commonObj.resolveUrl(path, isRoot, true) + "\" type=\"text/css\" rel=\"stylesheet\" />");
            }
        }
    }
    //系统默认需引用的文件
    //    common.using("core/jsCollect/collect.css");
    //    common.using("core/jsCollect/collect.js");
    //    common.using("core/expand.js");
    //    common.using("core/api.js");
    //    common.using("core/global.js");

    //获取当前Url
    commonObj.getCurrUrl = function (notEncode) {
        /// <summary>获取当前页面Url</summary>
        /// <param name="notEncode" type="bool">是否不需要url编码</param>
        var url = window.location.href.toLowerCase();
        if (notEncode !== true) {
            url = encodeURI(url);
        }
        return url;
    }



    //格式化且合并API参数
    commonObj.formatAPIParam = function (targetJson, sourceJson, isEncodeURIComponent) {
        /// <summary>格式化且合并参数</summary>
        /// <param name="targetJson" type="json">目标json(包含默认值)</param>
        /// <param name="sourceJson" type="json">源json(实际数据)</param>
        /// <param name="isEncodeURIComponent" type="bool">是否编码</param>
        targetJson = targetJson ? targetJson : {};
        sourceJson = sourceJson ? sourceJson : {};
        var sourceJsonTemp = $.extend({}, sourceJson);
        if (isEncodeURIComponent !== false) {
            function recursive(jsonData) {
                $.each(jsonData, function (index, item) {
                    if (commonObj.isJson(item)) {
                        recursive(item);
                    } else {
                        jsonData[index] = encodeURIComponent(item);
                    }
                });
            }
            recursive(sourceJsonTemp);
        }
        return $.extend({}, targetJson, sourceJsonTemp);
    };

    //判断是否为json数据
    commonObj.isJson = function (obj) {
        /// <summary>是否为json数据</summary>
        /// <param name="obj" type="obj">需判断的数据</param>
        var isjson = typeof (obj) == "object" && Object.prototype.toString.call(obj).toLowerCase() == "[object object]" && !obj.length;
        return isjson;
    }

    //post请求
    commonObj.post = function (url, data, success, failure, dataType) {
        /// <summary>发起异步post请求</summary>
        /// <param name="url" type="string">绝对Uri</param>
        /// <param name="data" type="json">传递的参数</param>
        /// <param name="success" type="function">执行成功后调用的方法</param>
        /// <param name="failure" type="function">执行失败后调用的方法</param>
        /// <param name="dataType" type="string">返回的数据类型(json|html|xml)</param>
        if (!dataType) { dataType = "json"; }
        var dt = commonObj.format("{0}{1}{2}{3}{4}", new Date().getDate(), new Date().getHours(), new Date().getMinutes(), new Date().getSeconds(), new Date().getMilliseconds());
        data = commonObj.formatAPIParam({ dt: dt }, data, false);
        //if($("#form1").length>0)
        var currUrl = window.location.href.toLowerCase();
        if (currUrl.indexOf("/zqxmanage/") != -1 && $.mask != undefined) { $("body").mask(); }
        $.post(url, data, function (result) {
            if (result && result.ret && result.ret == 1) {
                if (result.data === undefined) {
                    if (failure) {
                        failure("数据有误", "-100");
                    }
                } else if (success) {
                    success(result.data, result.msg);
                }
            } else if (failure) {
                failure(result.msg, result.errorcode);
            } else {
                commonObj.alert(result.msg);
            }
        }, dataType).complete(function () {
            if (currUrl.indexOf("/zqxmanage/") != -1 && $.mask != undefined) { $("body").mask("hide"); }
        });
    }

    //ajax请求
    commonObj.ajax = function (url, data, success, failure, settings) {
        /// <summary>发起ajax异/同步请求</summary>
        /// <param name="url" type="string">绝对Uri</param>
        /// <param name="data" type="json">传递的参数</param>
        /// <param name="success" type="function">执行成功后调用的方法</param>
        /// <param name="failure" type="function">执行失败后调用的方法</param>
        /// <param name="settings" type="json">相关设置</param>
        var defSetting = {
            containerId: undefined,
            async: false,
            type: "post",
            dataType: "json"
        };
        settings = commonObj.formatAPIParam(defSetting, settings, false);
        var dt = commonObj.format("{0}{1}{2}{3}{4}", new Date().getDate(), new Date().getHours(), new Date().getMinutes(), new Date().getSeconds(), new Date().getMilliseconds());
        data = commonObj.formatAPIParam({ dt: dt }, data, false);
        $.ajax({
            url: url,
            type: settings.type,
            async: settings.async,
            dataType: settings.dataType,
            data: data,
            beforeSend: function (XMLHttpRequest) {
                if (settings.containerId && $.mask != undefined) {
                    $("#" + settings.containerId).mask();
                }
            },
            complete: function (XMLHttpRequest, textStatus) {
                if (settings.containerId && $.mask != undefined) {
                    $("#" + settings.containerId).mask("hide");
                }
            },
            success: function (result, textStatus) {
                if (result && result.ret && result.ret == 1) {
                    if (result.data === undefined) {
                        if (failure) {
                            failure("数据有误", "-100");
                        }
                    } else if (success) {
                        success(result.data, result.msg);
                    }
                } else if (failure) {
                    failure(result.msg, result.errorcoed);
                } else {
                    commonObj.alert(result.msg);
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert("异步调用出现错误:" + textStatus);
            }
        });
    }

    //Jquery异步分页
    commonObj.paging = function (url, pagingContainerId, dataContainerId, param, success, failure, settings) {
        /// <summary>分页显示数据</summary>
        /// <param name="url" type="string">虚拟Uri</param>
        /// <param name="pagingContainerId" type="string">承载分页按钮的容器id</param>
        /// <param name="dataContainerId" type="function">承载数据的容器id</param>
        /// <param name="param" type="json">传递的参数</param>
        /// <param name="success" type="function">执行成功后调用的方法</param>
        /// <param name="failure" type="function">执行失败后调用的方法</param>
        /// <param name="settings" type="json">相关设置</param>
        if (!url) { alert("您需要指定异步调用的处理程序url！"); return false; }
        if ($("#" + pagingContainerId).length < 1) { alert("您指定的分页容器不存在！"); return false; }
        var defParam = {
            pageIndex: 1,
            pageSize: 10
        };
        param = commonObj.formatAPIParam(defParam, param, false);
        commonObj.ajax(url, param, function (data, msg) {
            if (data.rows === undefined || data.total === undefined) { alert("未找到分页实际数据！"); return false; }
            if (success) { success(data.rows, msg, data.total); }
            if (data.total > 0) {
                $("#" + pagingContainerId).show();
                var prevText = "上一页";
                var nextText = "下一页";
                if (settings) {
                    if (settings.prevText) { prevText = settings.prevText; }
                    if (settings.nextText) { nextText = settings.nextText; }
                }

                $("#" + pagingContainerId).pagination(data.total, {
                    callback: function (page_id, jq) {
                        param.pageIndex = page_id + 1;
                        commonObj.paging(url, pagingContainerId, dataContainerId, param, success, failure, settings);
                    },
                    prev_text: prevText,
                    next_text: nextText,
                    items_per_page: param.pageSize,
                    //num_display_entries: 5,
                    current_page: param.pageIndex - 1,
                    num_edge_entries: 1
                });
            } else {
                $("#" + pagingContainerId).hide();
            }
        }, failure, {
            containerId: dataContainerId,
            type: "post",
            dataType: "json"
        });
    }

    commonObj.format = function (source, params) {
        /// <summary>格式化数据(与C#中string的Format方法类似)</summary>
        /// <param name="source" type="string">待格式化的数据</param>
        /// <param name="params" type="object">参数系列（多个逗号分隔）</param>
        params = params === null ? "" : params;
        if (!params && params != 0) { return source; }
        if (arguments.length == 1)
            return function () {
                var args = $.makeArray(arguments);
                args.unshift(source);
                return commonObj.format.apply(this, args);
            };
        if (arguments.length > 2 && params.constructor != Array) {
            params = $.makeArray(arguments).slice(1);
        }
        if (params.constructor != Array) {
            params = [params];
        }
        $.each(params, function (i, n) {
            source = source.replace(new RegExp("\\{" + i + "\\}", "g"), n);
        });
        return source;
    };
    //摇晃窗口
    commonObj.windowShake = function (windowId) {
        /// <summary>摇晃窗口</summary>
        /// <param name="source" type="string">需摇晃的对象id</param>
        function s(id, pos) { g(id).left = pos + 'px'; }
        function g(id) { return document.getElementById(id).style; }
        function shake(id, a, d) { c = a.shift(); s(id, c); if (a.length > 0) { setTimeout(function () { shake(id, a, d); }, d); } else { try { g(id).position = 'static'; wp_attempt_focus(); } catch (e) { } } }
        var p = new Array(15, 30, 15, 0, -15, -30, -15, 0);
        p = p.concat(p.concat(p));
        g(windowId).position = 'relative';
        shake(windowId, p, 20);
    }

    //滚动到目标位置
    commonObj.scrollTop = function (target) {
        /// <summary>滚动到指定位置</summary>
        /// <param name="target" type="jqueryObj">jquery dom 对象</param>
        if (target) {
            $("html,body").animate({ scrollTop: target.offset().top }, 1000);
        }
    }

    commonObj.GetImageSize = function (url, f) {
        var img = new Image();
        img.src = url;

        if (img.complete) {
            f.call(img);
        } else {
            var oldstatus = window.status;
            window.status = "Loading...";
            img.onload = function () {
                f.call(img);
                window.status = oldstatus;
            }
        }
    }

    /*
    *   Description:图片等比缩放核心代码     
    *   Author:ZhangPan     
    *   Time:2009.11.12  23:25    
    *   使用方法:在图片标签的onload事件中输入ZoomImg(this,宽度,高度);
    */
    commonObj.zoomImg = function (imgControl, width, height, callback) {
        /// <summary>等缩放图片</summary>
        /// <param name="imgControl" type="imgObj">图片对象</param>
        /// <param name="width" type="int">目标宽</param>
        /// <param name="height" type="int">目标高</param>
        /// <param name="callback" type="function">等比缩放完毕后执行的方法</param>

        var uJ1 = imgControl;
        var $Ep2 = width;
        var v3 = height;
        var dj4 = new Image(); dj4["\x73\x72\x63"] = uJ1["\x73\x72\x63"];
        var imgSize = commonObj.GetImageSize(uJ1["\x73\x72\x63"], function () {
            var BjTBO5 = 0;
            var A6 = 0;
            var R7 = this.width;
            var IId8 = this.height;
            if (R7 > $Ep2) {
                BjTBO5 = $Ep2; A6 = BjTBO5 * IId8 / R7;
                if (A6 > v3) { BjTBO5 = v3 * BjTBO5 / A6; A6 = v3; } uJ1["\x77\x69\x64\x74\x68"] = BjTBO5; uJ1["\x68\x65\x69\x67\x68\x74"] = A6;
            }
            else if (IId8 > v3) {
                A6 = v3; BjTBO5 = A6 * R7 / IId8;
                if (BjTBO5 > $Ep2) { A6 = $Ep2 * A6 / BjTBO5; BjTBO5 = $Ep2; } uJ1["\x77\x69\x64\x74\x68"] = BjTBO5; uJ1["\x68\x65\x69\x67\x68\x74"] = A6;
            }
            else { uJ1["\x77\x69\x64\x74\x68"] = R7; uJ1["\x68\x65\x69\x67\x68\x74"] = IId8; }
            if (callback) { callback(); }
        });
    }

    //中英文字符串混合截取
    commonObj.subString = function (str, len) {
        /// <summary>中英文字符串混合截取</summary>
        /// <param name="str" type="string">需截取的字符串</param>
        /// <param name="len" type="int">截取字数</param>
        var str_length = 0;
        var str_len = 0;
        str_cut = new String();
        str_len = str.length;
        for (var i = 0; i < str_len; i++) {
            a = str.charAt(i);
            str_length++;
            if (escape(a).length > 4) {
                str_length++;
            }
            str_cut = str_cut.concat(a);
            if (str_length >= len) {
                str_cut = str_cut.concat("...");
                return str_cut;
            }
        }
        if (str_length < len) {
            return str;
        }
    }
    commonObj.alert = function (message) {
        /// <summary>弹出窗口</summary>
        /// <param name="message" type="string">弹出消息</param>
        if (typeof (Dialog) != "undefined" && typeof (Dialog) != undefined) {
            Dialog.alert(message);
        } else if (typeof (BootstrapDialog) != "undefined" && typeof (BootstrapDialog) != undefined) {
            BootstrapDialog.extAlert(message, BootstrapDialog.TYPE_INFO);
        } else if (typeof (mobileAlert) =="function") {
            mobileAlert(message);
        } else { alert(message); }
    }
    commonObj.alertJson = function (message) {
        /// <summary>弹出窗口</summary>
        /// <param name="message" type="string">弹出消息</param>
        alert(JSON.stringify(message));
    }

    commonObj.urlToJson = function (url) {
        url = url.substring(url.indexOf("?") + 1);
        var querys = url.split("&");
        var queryJson = {};
        for (var i = 0; i < querys.length; i++) {
            var item = querys[i].split("=");
            queryJson[item[0].toString()] = item[1];
        }
        return queryJson;
    }

    //限制多行文本框字数，需引用 jquery.inputlimitor.1.0.min.js
    //controls：需要限制的文本框[jquery对象]，如：$("#txtContent")
    //limit：最大可输入字数（默认值:100）
    commonObj.inputlimitor = function (controls, limit) {
        limit = limit ? limit : 100;
        controls.inputlimitor({ limit: 100, remText: "您还可输入 %n 个字符", limitText: "共可输入 %n 个字符" });
    }

    //复制内容到剪切板
    commonObj.copyToClipboard = function (txt) {
        if (window.clipboardData) {
            window.clipboardData.clearData();
            window.clipboardData.setData("Text", txt);
            //alert("复制成功！")
        } else if (navigator.userAgent.indexOf("Opera") != -1) {
            window.location = txt;
            //alert("复制成功！");
        } else if (window.netscape) {
            try {
                netscape.security.PrivilegeManager.enablePrivilege("UniversalXPConnect");
            } catch (e) {
                alert("被浏览器拒绝！\n请在浏览器地址栏输入'about:config'并回车\n然后将 'signed.applets.codebase_principal_support'设置为'true'");
            }
            var clip = Components.classes['@mozilla.org/widget/clipboard;1'].createInstance(Components.interfaces.nsIClipboard);
            if (!clip)
                return;
            var trans = Components.classes['@mozilla.org/widget/transferable;1'].createInstance(Components.interfaces.nsITransferable);
            if (!trans)
                return;
            trans.addDataFlavor('text/unicode');
            var str = new Object();
            var str = Components.classes["@mozilla.org/supports-string;1"].createInstance(Components.interfaces.nsISupportsString);
            var copytext = txt;
            str.data = copytext;
            trans.setTransferData("text/unicode", str, copytext.length * 2);
            var clipid = Components.interfaces.nsIClipboard;
            if (!clip)
                return false;
            clip.setData(trans, null, clipid.kGlobalClipboard);
            //alert("复制成功！")
        } else {
            alert("当前浏览器不支持自动复制，请手动选择文本按Ctrl+C复制！")
        }
    }

    commonObj.newGuid = function () {
        var guid = "";
        for (var i = 1; i <= 32; i++) {
            var n = Math.floor(Math.random() * 16.0).toString(16);
            guid += n;
            if ((i == 8) || (i == 12) || (i == 16) || (i == 20))
                guid += "-";
        }
        return guid;
    }

    commonObj.serializeJsonByFrom = function (formId) {
        var dataArray = $("#" + formId).not(":checkbox,:radio").serializeArray();
        var cbxAndRadio = $("#" + formId).find("input:checkbox:checked,input:radio:checked");
        var cbxAndRadioData = {};
        if (cbxAndRadio.length > 0) {
            cbxAndRadio.each(function (index, item) {
                var currObj = $(item);
                var currName = currObj.attr("name");
                var currVal = currObj.attr("value");
                if (cbxAndRadioData[currName]) {
                    cbxAndRadioData[currName] = (cbxAndRadioData[currName] + "," + currVal);
                } else {
                    cbxAndRadioData[currName] = currVal;
                }
            });
            for (var key in cbxAndRadioData) {
                dataArray.push({ name: key, value: cbxAndRadioData[key] });
            }
        }
        var serializeObj = {};
        $.each(dataArray, function (index, item) {
            serializeObj[item.name] = item.value;
        });
        return serializeObj;
    }

    //通过json数据填充表单
    commonObj.fillFromByJson = function (formId, jsonData) {
        if (!commonObj.isJson(jsonData)) { return false; }
        var formControl = $("#" + formId);
        for (var key in jsonData) {
            //跳过以N_打头的字段（表示非数据库字段）
            if (key.indexOf("N_") != -1) { continue;}
            //处理值
            var value = jsonData[key];
            //获取控件
            var txtControl = formControl.find(common.format("input:text[name='{0}']", key));
            var pwdControl = formControl.find(common.format("input:password[name='{0}']", key));
            var ddlControl = formControl.find(common.format("select[name='{0}']", key));
            var radControl = formControl.find(common.format("input:radio[name='{0}'][value='{1}']", key, value));
            var cbxControl = formControl.find(common.format("input:checkbox[name='{0}'][value='{1}']", key, value));
            var txtAreaControl = formControl.find(common.format("textarea[name='{0}']", key));
            var hiddenControl = formControl.find(common.format("input:hidden[name='{0}']", key));
            //赋值
            if (txtControl.length > 0) {
                txtControl.val(value)
            }
            if (pwdControl.length > 0) {
                pwdControl.val(value)
            }
            if (ddlControl.length > 0) {
                ddlControl.val(value)
            }
            if (radControl.length > 0) {
                radControl.attr("checked", "checked");
            }
            if (cbxControl.length > 0) {
                cbxControl.attr("checked", "checked");
            }
            if (txtAreaControl.length > 0) {
                txtAreaControl.val(value)
            }
            if (hiddenControl.length > 0) {
                hiddenControl.val(value)
            }
        }
    }
})(common);




/********************************************************************************************************
* Description: 公共验证方案
* Add Author: ZhangPan
* Create Date: 2012-08-21 11:59:37
*********************************************************************************************************/
common.validate = function () { };
(function (validateObj) {
    validateObj.regexEnum = {
        integer: "^-?[0-9]\\d*$", 				//整数（包括0）
        integerPositive: "^[0-9]\\d*$", 		//正整数（包括0）
        integerNegative: "^-[0-9]\\d*$", 		//负整数（不包括0）
        number: "^([+-]?)\\d*\\.?\\d+$", 		//数字（包括0）
        numberPositive: "^[0-9]\\d*|0$", 		//正数（包括0）
        numberNegative: "^-[1-9]\\d*|0$", 		//负数（不包括0）
        decimalOrInteger: "^\\d+(\\.\\d+)?$",     //浮点数或整数
        decmal: "^([+-]?)\\d*\\.\\d+$", 		//浮点数
        decmalPositive: "^[1-9]\\d*.\\d*|0.\\d*[1-9]\\d*|0?.0+|0$", //正浮点数
        decmalNegative: "^(-([1-9]\\d*.\\d*|0.\\d*[1-9]\\d*))|0?.0+|0$", //负浮点数
        decmal6: "^(([0-9]+\.[0-9]{1})|([0-9]*[1-9][0-9]*\.[0-9]{1})|([0-9]*[0-9]{1}))$", //非负浮点数（正浮点数 + 0，保留一位小数）

        email: "^\\w+((-\\w+)|(\\.\\w+))*\\@[A-Za-z0-9]+((\\.|-)[A-Za-z0-9]+)*\\.[A-Za-z0-9]+$", //邮件
        color: "^[a-fA-F0-9]{6}$", 			//颜色
        url: "^http[s]?:\\/\\/([\\w-]+\\.)+[\\w-]+([\\w-./?%&=]*)?$", //url
        chinese: "^[\\u4E00-\\u9FA5\\uF900-\\uFA2D]+$", 				//仅中文
        ascii: "^[\\x00-\\xFF]+$", 			//仅ACSII字符
        postcode: "^\\d{6}$", 					//邮编
        mobile: "^(13|15|18|14|17)[0-9]{9}$", 			//手机
        ip4: "^(25[0-5]|2[0-4]\\d|[0-1]\\d{2}|[1-9]?\\d)\\.(25[0-5]|2[0-4]\\d|[0-1]\\d{2}|[1-9]?\\d)\\.(25[0-5]|2[0-4]\\d|[0-1]\\d{2}|[1-9]?\\d)\\.(25[0-5]|2[0-4]\\d|[0-1]\\d{2}|[1-9]?\\d)$", //ip地址
        notempty: "^\\S+$", 					//非空
        picture: "(.*)\\.(jpg|bmp|gif|ico|pcx|jpeg|tif|png|raw|tga)$", //图片
        rar: "(.*)\\.(rar|zip|7zip|tgz)$", 							//压缩文件
        date: "^\\d{4}(\\-|\\/|\.)\\d{1,2}\\1\\d{1,2}$", 				//日期
        qq: "^[1-9]*[1-9][0-9]*$", 			//QQ号码
        tel: "^(([0\\+]\\d{2,3}-)?(0\\d{2,3})-)?(\\d{7,8})(-(\\d{3,}))?$", //电话号码的函数(包括验证国内区号,国际区号,分机号)
        telMore: "^((([0\\+]\\d{2,3}-)?(0\\d{2,3})-)?(\\d{7,8})(-(\\d{3,}))?)(/(([0\\+]\\d{2,3}-)?(0\\d{2,3})-)?(\\d{7,8})(-(\\d{3,}))?)*$", //多个电话号码的函数(包括验证国内区号,国际区号,分机号),以/分隔
        username: "^[a-zA-Z]{1}([a-zA-Z0-9]|[._]){5,19}$", 					//以字母开头，由数字、26个英文字母或下划线组成的6-20位字符
        letter: "^[A-Za-z]+$", 				//字母
        letter_u: "^[A-Z]+$", 				//大写字母
        letter_l: "^[a-z]+$", 				//小写字母
        idcard: "^[1-9]([0-9]{14}|[0-9]{17})$",	//身份证
        time: "^([0-1]?[0-9]|2[0-3]):([0-5][0-9]):([0-5][0-9])$"
    }


    //自定义正则表达式验证
    validateObj.regexMatch = function (str, regex) {
        /// <summary>自定义正则表达式验证</summary>
        /// <param name="str" type="string">待验证的字符串</param>
        /// <param name="regex" type="string">正则表达式</param>
        var r = str.match(regex);
        if (r == null) return false;
        return true;
    }
    //验证身份证号是否合法
    validateObj.validateIDCare = function (sId) {
        /// <summary>验证身份证号是否合法</summary>
        /// <param name="sId" type="string">身份证号</param>
        var aCity = { 11: "北京", 12: "天津", 13: "河北", 14: "山西", 15: "内蒙古", 21: "辽宁", 22: "吉林", 23: "黑龙江", 31: "上海", 32: "江苏", 33: "浙江", 34: "安徽", 35: "福建", 36: "江西", 37: "山东", 41: "河南", 42: "湖北", 43: "湖南", 44: "广东", 45: "广西", 46: "海南", 50: "重庆", 51: "四川", 52: "贵州", 53: "云南", 54: "西藏", 61: "陕西", 62: "甘肃", 63: "青海", 64: "宁夏", 65: "新疆", 71: "台湾", 81: "香港", 82: "澳门", 91: "国外" }

        var iSum = 0;
        var info = "";
        if (!/^\d{17}(\d|x)$/i.test(sId)) return "你输入的身份证长度或格式错误";
        sId = sId.replace(/x$/i, "a");
        if (aCity[parseInt(sId.substr(0, 2))] == null) return "你的身份证地区非法";
        sBirthday = sId.substr(6, 4) + "-" + Number(sId.substr(10, 2)) + "-" + Number(sId.substr(12, 2));
        var d = new Date(sBirthday.replace(/-/g, "/"));
        if (sBirthday != (d.getFullYear() + "-" + (d.getMonth() + 1) + "-" + d.getDate())) return "身份证上的出生日期非法";
        for (var i = 17; i >= 0; i--) iSum += (Math.pow(2, i) % 11) * parseInt(sId.charAt(17 - i), 11);
        if (iSum % 11 != 1) return "你输入的身份证号非法";
        return true;
    }

    //短时间，形如 (13:04:06)
    validateObj.isTime = function (str) {
        /// <summary>验证是否为：短时间，形如 (13:04:06)</summary>
        /// <param name="str" type="string">待验证的字符串</param>
        var a = str.match(/^(\d{1,2})(:)?(\d{1,2})\2(\d{1,2})$/);
        if (a == null) { return false }
        if (a[1] > 24 || a[3] > 60 || a[4] > 60) {
            return false;
        }
        return true;
    }

    //短日期，形如 (2003-12-05)
    validateObj.isDate = function (str) {
        /// <summary>验证是否为：短日期，形如 (2003-12-05)</summary>
        /// <param name="str" type="string">待验证的字符串</param>
        var r = str.match(/^(\d{1,4})(-|\/)(\d{1,2})\2(\d{1,2})$/);
        if (r == null) return false;
        var d = new Date(r[1], r[3] - 1, r[4]);
        return (d.getFullYear() == r[1] && (d.getMonth() + 1) == r[3] && d.getDate() == r[4]);
    }

    //正整数
    validateObj.isIntege = function (str) {
        /// <summary>验证是否为：正整数</summary>
        /// <param name="str" type="string">待验证的字符串</param>
        var r = str.match(validateObj.regexEnum.integer);
        if (r == null) return false;
        return true;
    }

    //长时间，形如 (2003-12-05 13:04:06)
    validateObj.isDateTime = function (str) {
        /// <summary>验证是否为：长时间，形如 (2003-12-05 13:04:06)</summary>
        /// <param name="str" type="string">待验证的字符串</param>
        var reg = /^(\d{1,4})(-|\/)(\d{1,2})\2(\d{1,2}) (\d{1,2}):(\d{1,2}):(\d{1,2})$/;
        var r = str.match(reg);
        if (r == null) return false;
        var d = new Date(r[1], r[3] - 1, r[4], r[5], r[6], r[7]);
        return (d.getFullYear() == r[1] && (d.getMonth() + 1) == r[3] && d.getDate() == r[4] && d.getHours() == r[5] && d.getMinutes() == r[6] && d.getSeconds() == r[7]);
    }

    //GUID正确性（排除Guid.Empty）
    validateObj.isGuid = function (str) {
        /// <summary>验证是否为：GUID正确性（排除Guid.Empty）</summary>
        /// <param name="str" type="string">待验证的字符串</param>
        if (str == "" || str == null || str == undefined || str == NaN || str == "00000000-0000-0000-0000-000000000000") {
            return false;
        }
        return true;
    }

    //是否为空字符串或null
    validateObj.isEmptyOrNull = function (str) {
        /// <summary>验证是否为：空字符串或null</summary>
        /// <param name="str" type="string">待验证的字符串</param>
        str = $.trim(str);
        if (str == "" || str == null || str == undefined || str == NaN) {
            return true;
        }
        return false;
    }

    //判断字符串是否达到指定长度
    validateObj.compareLength = function (str, strLenght) {
        /// <summary>判断字符串是否达到指定长度</summary>
        /// <param name="str" type="string">待验证的字符串</param>
        /// <param name="strLenght" type="string">待验证的字符串</param>
        str = $.trim(str);
        if (str != "" && str != null && str != undefined && str != NaN) {
            if (str.length >= strLenght)
                return true;
            else
                return false;
        }
        return false;
    }

    //是否为0
    validateObj.isZero = function (str) {
        /// <summary>验证是否为：0</summary>
        /// <param name="str" type="string">待验证的字符串</param>
        if (str === "0" || str === 0) {
            return true;
        }
        return false;
    }

    //是否为Email
    validateObj.isEmail = function (str) {
        /// <summary>验证是否为：电子邮件</summary>
        /// <param name="str" type="string">待验证的字符串</param>
        var r = str.match(validateObj.regexEnum.email);
        if (r == null) return false;
        return true;
    }

    //是否为Mobile
    validateObj.isMobile = function (str) {
        /// <summary>验证是否为：手机号</summary>
        /// <param name="str" type="string">待验证的字符串</param>
        var r = str.match(validateObj.regexEnum.mobile);
        if (r == null) return false;
        return true;
    }

    //是否为邮政编码
    validateObj.isPostCode = function (str) {
        /// <summary>验证是否为：邮政编码</summary>
        /// <param name="str" type="string">待验证的字符串</param>
        var r = str.match(validateObj.regexEnum.postcode);
        if (r == null) return false;
        return true;
    }

    //检查指定单选框是否已选定
    validateObj.isRedioSelected = function (radioName) {
        /// <summary>检查指定单选框是否已选定</summary>
        /// <param name="radioName" type="string">单选框name</param>
        var count = $("input:radio[name=" + radioName + "]:checked").length;
        return count > 0;
    }

    //是否为时间
    validateObj.isTime = function (str) {
        /// <summary>验证是否为：时间</summary>
        /// <param name="str" type="string">待验证的字符串</param>
        var r = str.match(validateObj.regexEnum.time);
        if (r == null) return false;
        return true;
    }
})(common.validate);



/********************************************************************************************************
* Description: 公共控件
* Add Author: ZhangPan
* Create Date: 2012-09-26 19:28:27
* End Update Author:
* End Update Date:
*********************************************************************************************************/
common.controls = function () { };
(function (controlsObj) {
    controlsObj.FCK = function (containerId) {
        this.containerId = containerId;
        this.Init = function (defaultValue, isDefaultSkin, isAllToolbar) {
            var spFCK1 = new FCKeditor(this.containerId);
            spFCK1.Height = "280px";
            spFCK1.Width = "98%";
            if (!isDefaultSkin) {
                spFCK1.Config = { SkinPath: common.resolveUrl("/fckeditor/editor/skins/famfamfamAluminum/", true) }
            }
            if (defaultValue) {
                spFCK1.Value = defaultValue;
            }
            if (isAllToolbar !== true) {
                spFCK1.Height = "200px";
                spFCK1.ToolbarSet = "Simplify";
            }
            $("#" + this.containerId).html(spFCK1.CreateHtml());
        }

        this.GetValue = function (isHtml) {
            var spFCK1 = FCKeditorAPI.GetInstance(this.containerId);
            return spFCK1.GetXHTML(isHtml);
        }

        this.SetValue = function (value, isHtml) {
            var spFCK1 = FCKeditorAPI.GetInstance(this.containerId);
            spFCK1.SetHTML(value, isHtml);
        }
    }
})(common.controls);

//级联下拉框扩展
common.bindselect = function (obj, callback) {
    if (obj.length == 1) {
        getJson("#" + obj[0]);
    }
    else {
        for (var i = 0; i < obj.length; i++) {
            $(document).off("change", "#" + obj[i]).on("change", "#" + obj[i], function (evt) {
                var index = $.inArray($(this).attr("id"), obj);
                var j = index + 1;
                if (j < obj.length) {
                    getJson("#" + obj[j], $(this).val());
                }
            });
        }
    }
    function getJson(id, val) {
        val = val ? val : 0;
        default_value = $(id).attr("defualt-value");
        var selected = "";
        $.ajax({
            type: "post",
            url: $(id).attr("data-url"),
            data: { id: val },
            cache: false,
            //async: false,
            dataType: "json",
            success: function (data) {
                $(id).empty();
                for (var i = 0; i < data.length; i++) {
                    var data_value = data[i][$(id).attr("data-value")];
                    if (default_value == data_value) {
                        selected = " selected ";
                    } else {
                        selected = "";
                    }
                    $(id).append("<option " + selected + " value='" + data_value + "'>" + data[i][$(id).attr("data-text")] + "</option>");
                };
                if (callback) {
                    callback();
                }
                $(id).trigger("change");
            }
        });
    }
    $(function () {
        $("#" + obj[0]).trigger("change");
    })
}
var util = {};
//公共弹出框
util.alert = function (content) {
    //信息框
    layer.open({
        content: content
      , btn: '我知道了'
    });
}
util.confirm = function (content, callback) {
    layer.open({
        content: content
        , btn: ['取消', '确定']
        , yes: function (index) {
            layer.close(index);
        }, no: function (index) {
            if (callback != undefined) {
                callback();
            }                
            layer.close(index);
        }
    });
}
util.info = function (content) {
    layer.open({
        title: [
          '信息',
          'background-color: #3387d7; color:#fff;'
        ],
        content: content, btn: '我知道了'
    });
}
