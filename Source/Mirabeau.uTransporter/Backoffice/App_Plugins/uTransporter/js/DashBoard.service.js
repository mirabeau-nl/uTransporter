angular.module("umbraco.services").factory("uTransporterService", function ($http, $q) {
    return {
        startSync: function () {
            var defered = $q.defer();

            $http.post("/umbraco/backoffice/API/UTransporter/StartSync")
                .success(defered.resolve)
                .error(defered.resolve);

            return defered.promise;
        },

        getLog: function () {
            var defered = $q.defer();
            $http({ method: "GET", url: "/umbraco/backoffice/API/UTransporter/GetLog?path=~/App_Data/Logs/UmbracoTraceLog.txt" }).success(function (result) {
                defered.resolve(result);
            });

            return defered.promise;
            //return $http.get("/umbraco/backoffice/API/uTransporter/GetLog?path=~/App_Data/Logs/UmbracoTraceLog.txt");
        },

        generate: function () {
            var defered = $q.defer();

            $http.post("/umbraco/backoffice/API/UTransporter/StartUpGeneration")
                .success(defered.resolve)
                .error(defered.resolve);

            return defered.promise;
        },

        startRemoveDocumentTypes: function() {
            var defered = $q.defer();

            $http.post("/umbraco/backoffice/API/UTransporter/StartRemoveDocumentTypes")
                .success(defered.resolve)
                .error(defered.resolve);

            return defered.promise;
        },

        downloadLog: function () {
            window.location.href = "/umbraco/backoffice/API/UTransporter/DownloadLog";
        },

        dryRun: function() {
            var defered = $q.defer();

            $http.post("/umbraco/backoffice/API/UTransporter/DryRun")
                .success(defered.resolve)
                .error(defered.resolve);

            return defered.promise;
        }
    };
});
