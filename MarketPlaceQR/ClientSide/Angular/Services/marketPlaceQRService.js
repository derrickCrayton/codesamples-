(function () {
    "use strict";

    angular.module(APPNAME)
    .factory('$marketplaceService', marketplaceServiceFactory);

    marketplaceServiceFactory.$inject = ['$baseService', '$sabio'];

    function marketplaceServiceFactory($baseService, $sabio) {

        var marketplaceServiceObject = sabio.services.marketplace;

        var newService = $baseService.merge(true, {}, marketplaceServiceObject, $baseService);

        return newService;
    };
})();