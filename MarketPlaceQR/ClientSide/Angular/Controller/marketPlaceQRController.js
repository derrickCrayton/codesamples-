(function () {
    "use strict";

    angular.module(APPNAME)
    .controller('marketplaceController', MarketPlaceController);

    MarketPlaceController.$inject = ['$scope', '$baseController',
        "$marketplaceService", "$googleMapServiceTemp", '$addressService',
        "$supplierService", '$alertService'];

    function MarketPlaceController(
        $scope, $baseController
        , $marketplaceService, $googleMapServiceTemp, $addressService
        , $supplierService, $alertService) {

        var vm = this;
        vm.items = null;

        vm.$marketplaceService = $marketplaceService;
        vm.$googleMapService = $googleMapServiceTemp;
        vm.$addressService = $addressService;
        vm.$supplierService = $supplierService;
        vm.$alertService = $alertService;
        vm.$scope = $scope;

        vm.companyId = $('#PAGECOMPANY').val();

        vm.startSearch = _startSearch;
        vm.submitSearch = _submitSearch;

        vm.receivedItems = null;
        vm.addressBook = null;
        vm.quoteRequestId = null;
        vm.$scope.estimation = null;

        vm.dueDate = null;
        vm.status = null;
        vm.details = null;
        vm.name = null;
        vm.distance = null;

        vm.address = null;
        vm.addressName = null;
        vm.city = null;
        vm.state = null;
        vm.zipcode = null;


        vm.useCompanySupply = false;
        vm.search = false;
        vm.addPopUp = "popover-dismiss";
        vm.noResultsFound = true;

        _getAddressBookByCompanyId();

        /***********************************************
        Simulated Database Information (Start)
        ************************************************/

        vm.sampleCatalog = sabio.page.masterformat;

        vm.catLocations = vm.addressBook;

        vm.sampleRadius = [
            {
                id: 12,
                range: "within 10 miles",
                value: 10

            },

            {
                id: 13,
                range: "within 25 miles",
                value: 25
            },

            {
                id: 14,
                range: "within 50 miles",
                value: 50
            },

            {
                id: 15,
                range: "within 75 miles",
                value: 75
            },

            {
                id: 16,
                range: "within 100 miles",
                value: 100
            },

            {
                id: 17,
                range: "100+ miles",
                value: 999999
            },

        ];

        vm.catRadius = vm.sampleRadius[0].value;
        vm.categoryIds = [];

        /*****************************************************
        Simulated Database Information (End)
        ******************************************************/

        $baseController.merge(vm, $baseController);

        vm.notify = vm.$supplierService.getNotifier($scope);
        vm.notify = vm.$marketplaceService.getNotifier($scope);




        //....// =====================================================
        function _receiveSearchItems(data) {
            console.log('_receiveSearchItems works', data);

            var searchResults = data.items;

            if (searchResults != null && typeof searchResults === "object" && searchResults.length > 0) {

                for (var i = 0; i < searchResults.length; i++) {

                    searchResults[i].estimations = "$" + searchResults[i].estimation;
                    var Date = searchResults[i].dueDate;
                    searchResults[i].dueDates = moment(Date).format("L");

                    searchResults[i].address = searchResults[i].address1 + ", " + searchResults[i].city + ", " + searchResults[i].state + " " + searchResults[i].zipCode;
                    searchResults[i].addressName = searchResults[i].address1;
                    searchResults[i].distances = Math.round(searchResults[i].distance);

                    searchResults[i].url = "/quoterequest/manage/" + searchResults[i].quoteRequestId;

                }
            } else {

                vm.$alertService.error('please try another search', 'No Search Results Found!');
            }

            vm.notify(function () {

                vm.receivedItems = searchResults;

            });

            setTimeout(function () {
                $(window).trigger('resize');
                //console.log('trigger resize running');
            }, 200);


            $(".marketplace").each(function () {

                var gmarkers = [];

                for (i = 0; i < gmarkers.length; i++) {
                    gmarkers[i].setMap(null);
                }

                var currentMapId = $(this).attr('id');
                var thisMap = vm.$googleMapService.init();

                thisMap.init(currentMapId);

                var searchArray = [
                {
                    latitude: $(this).data('latitude'),
                    longitude: $(this).data('longitude')
                },

                {
                    latitude: vm.catLocations.Lat,
                    longitude: vm.catLocations.Lng
                }

                ];

                if (searchArray) {

                    var myCanvas = document.getElementById(currentMapId);
                    var myCenter = new google.maps.LatLng(33.6189, -117.9289);
                    var myOptions = { center: myCenter, zoom: 12 };
                    var myMap = new google.maps.Map(myCanvas, myOptions);

                    for (i = searchArray.length - 1; i >= 0; i--) {

                        myCenter = new google.maps.LatLng(searchArray[i].latitude, searchArray[i].longitude);
                        var marker = new google.maps.Marker({ position: myCenter, clickable: true });

                        marker.setMap(myMap);
                        gmarkers.push(marker);

                    }

                    myMap.panTo(myCenter);

                    var bounds = new google.maps.LatLngBounds();
                    for (var i = 0; i < gmarkers.length; i++) {
                        bounds.extend(gmarkers[i].getPosition());
                    }

                    myMap.fitBounds(bounds);
                }


            });


        };

        //....// =====================================================
        function _receiveAddresses(data) {
            console.log("received company addresses", data);

            var addressOptionArray = [];
            //Build Address Line
            for (var prop in data.item) {
                if (data.item[prop] != null && data.item[prop].length > 0) {

                    var property = data.item[prop]

                    for (var i = 0; i < property.length; i++) {

                        var addressObjectTemplate = {

                            id: property[i].addressId,
                            address: property[i].address1 + ", " + property[i].city + ", " + property[i].state + " " + property[i].zipCode,
                            //Build Lat/Long Value
                            latlong: {
                                Lat: property[i].latitude,
                                Lng: property[i].longitude
                            }
                        };

                        addressOptionArray.push(addressObjectTemplate);
                    }
                }
            }

            //return ngoption table

            vm.addressBook = addressOptionArray;
        }

        function _onError(jqXhr, error) {
            console.log(error);
        };

        function _getAddressBookByCompanyId() {
            vm.$addressService.getAddressBookByCompanyId(vm.companyId, _receiveAddresses, _onError)
            console.log("companyId: ", vm.companyId);
        }


        //....// =====================================================
        function getSettingsForUserSuccess(data) {

            // Create four new items in the data object for each item
            // Loop through the data.items
            if (data.items !== null && typeof data.items === 'object' && data.items.length > 0) {
                for (var i = 0; i < data.items.length; i++) {

                    vm.categoryIds.push(data.items[i].categoryId);
                }
            }

            var categoryidList = [vm.catCatalog]; //<---
            if (vm.useCompanySupply == true) {

                //var categoryidList = [
                //        51200,
                //        33543,
                //        30000,
                //        103
                //];


                var categoryidList = vm.categoryIds;
            }

            var searchPayload = {
                latpoint: vm.catLocations.Lat,
                lngpoint: vm.catLocations.Lng,
                radius: vm.catRadius
            }


            vm.$marketplaceService.getByCategorySearch(categoryidList, searchPayload, _receiveSearchItems, _onError);

            vm.search = true;
            $('#chosenProducts').val('').trigger('chosen:updated');
            vm.catCatalog = false;
            vm.useCompanySupply = false;
        };

        //....// =====================================================
        function _startSearch() {
            console.log('startSearch works');

            vm.$supplierService.getSettingsByUserId(getSettingsForUserSuccess, _onError);

        };

        vm.$scope.$watch('estimation', function () {
            if (vm.$scope.estimation != null) {
                console.log("estimation changed");
            }
        });

        function _submitSearch(isValid) {
            console.log("search works");
            if (isValid) {
                _startSearch();
            }
        };

    }

})();