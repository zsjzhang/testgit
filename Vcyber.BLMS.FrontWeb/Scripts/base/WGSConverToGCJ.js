function WGSConverToGCJ() {
    this.transformFromWGSToGCJ = transformFromWGSToGCJ;
    this.transformLat = transformLat;
    this.transformLon = transformLon;
}
function transformFromWGSToGCJ(longitude, latitude) {
    var a = 6378245.0;
    var ee = 0.00669342162296594323;
    var dLat = transformLat(longitude - 105.0,
                    latitude - 35.0);
    var dLon = transformLon(longitude - 105.0,
                    latitude - 35.0);
    var radLat = latitude / 180.0 * Math.PI;
    var magic = Math.sin(radLat);
    magic = 1 - ee * magic * magic;
    var sqrtMagic = Math.sqrt(magic);
    dLat = (dLat * 180.0) / ((a * (1 - ee)) / (magic * sqrtMagic) * Math.PI);
    dLon = (dLon * 180.0) / (a / sqrtMagic * Math.cos(radLat) * Math.PI);
    var lon = longitude + dLon;
    var lat = latitude + dLat;
    var result = [];
    result.push(lon);
    result.push(lat);
    return result;
};

function transformLat(x, y) {
    var ret = -100.0 + 2.0 * x + 3.0 * y + 0.2 * y * y + 0.1 * x * y
                    + 0.2 * Math.sqrt(x > 0 ? x : -x);
    ret += (20.0 * Math.sin(6.0 * x * Math.PI) + 20.0 * Math.sin(2.0 * x
                    * Math.PI)) * 2.0 / 3.0;
    ret += (20.0 * Math.sin(y * Math.PI) + 40.0 * Math.sin(y / 3.0
                    * Math.PI)) * 2.0 / 3.0;
    ret += (160.0 * Math.sin(y / 12.0 * Math.PI) + 320 * Math.sin(y
                    * Math.PI / 30.0)) * 2.0 / 3.0;
    return ret;
};

function transformLon(x, y) {
    var ret = 300.0 + x + 2.0 * y + 0.1 * x * x + 0.1 * x * y + 0.1
                    * Math.sqrt(x > 0 ? x : -x);
    ret += (20.0 * Math.sin(6.0 * x * Math.PI) + 20.0 * Math.sin(2.0 * x
                    * Math.PI)) * 2.0 / 3.0;
    ret += (20.0 * Math.sin(x * Math.PI) + 40.0 * Math.sin(x / 3.0
                    * Math.PI)) * 2.0 / 3.0;
    ret += (150.0 * Math.sin(x / 12.0 * Math.PI) + 300.0 * Math.sin(x
                    / 30.0 * Math.PI)) * 2.0 / 3.0;
    return ret;
};