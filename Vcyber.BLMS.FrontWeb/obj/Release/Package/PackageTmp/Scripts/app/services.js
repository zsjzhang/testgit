
var vcyberpayServices = angular.module('vcyberpayServices', ['ngResource']);
vcyberpayServices.factory("vcyberpayServices", function ($resource) {
    return $resource(
        "/api/Product/:id",
        { id: "@id" },
        {
            "update": { method: "PUT" }

        }
    );
});
