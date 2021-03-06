﻿angular.module('umbraco').controller("uTransporter.uTransporterController",
    function ($scope, uTransporterService, $filter, $routeParams, $timeout, dialogService) {
        $scope.id = $routeParams.id;

        // Show spinner on button click (all button-actions perform AJAX requests)
        $("button").click(function () {
            $scope.resetAllMessages();
            $(".spinner").show("slow");
        });

        $scope.startSync = function () {
            $(".spinner").hide();

            dialogService.open({
                template: '/App_Plugins/uTransporter/ConfirmAction.html?t=' + new Date().getTime(),
                callback: function (confirmed) {

                    if (!confirmed) {
                        return;
                    }

                    $(".spinner").show("slow");
                    uTransporterService.startSync().then(function (syncResult) {
                        $(".spinner").hide("slow");

                        if (syncResult.successful) {
                            $scope.syncResult = syncResult;
                            $scope.result = syncResult.message;
                            $scope.style = "success";
                        } else {
                            $scope.style = "failed";
                            $scope.result = syncResult.message;
                            $scope.resultText = "Please consult the Umbraco Sync log for more information.";
                        }

                        // Give the server some time to write to log file, 2.5s seems ok.
                        if ($scope.showLog) {
                            setTimeout(function () {
                                $scope.getLog();
                            }, 2500);
                        }
                    });
                }
            });
        };

        $scope.startGenerate = function () {
            $(".spinner").hide();

            dialogService.open({
                template: '/App_Plugins/uTransporter/ConfirmAction.html?t=' + new Date().getTime(),
                callback: function (confirmed) {

                    if (!confirmed) {
                        return;
                    }

                    $(".spinner").show("slow");
                    uTransporterService.generate().then(function (generateResult) {
                        $(".spinner").hide("slow");
                        if (generateResult.successful) {
                            $scope.generateStyle = "success";
                            $scope.generationResult = generateResult;
                            $scope.generationMessage = generateResult.message;
                        } else {
                            $scope.generateStyle = "failed";
                            $scope.resultText = "Please consult the Umbraco Sync log for more information.";
                        }
                    });
                }
            });
        };

        $scope.startRemoveDocumentTypes = function () {
            $(".spinner").hide();

            dialogService.open({
                template: '/App_Plugins/uTransporter/ConfirmAction.html?t=' + new Date().getTime(),
                callback: function (confirmed) {

                    if (!confirmed) {
                        return;
                    }

                    $(".spinner").show("slow");
                    uTransporterService.startRemoveDocumentTypes().then(function (removeResult) {
                        $(".spinner").hide("slow");

                        if (removeResult.successful) {
                            $scope.removeResult = removeResult;
                            $scope.removeStyle = "remove-success";
                            $scope.removeResultMessage = removeResult.message;
                        } else {
                            $scope.removeStyle = "remove-failed";
                            $scope.removeResultMessage = syncResult.message;
                            $scope.removeResultMessageUnderline = "Please consult the Umbraco Sync log for more information.";
                        }
                    });
                }
            });
        };

        $scope.resetAllMessages = function () {
            $scope.removeStyle = "";
            $scope.removeResult = "";
            $scope.removeResultMessage = "";
            $scope.removeResultMessageUnderline = "";

            $scope.style = "";
            $scope.result = "";
            $scope.resultText = "";

            $scope.generateStyle = "";
            $scope.generationMessage = "";
            $scope.generationResult = "";

            $scope.dryRunStyle = "";
            $scope.dryRunResult = "";
            $scope.dryRunText = "";

            $scope.showLog = false;
        };

        $scope.help = function () {
            dialogService.open({
                template: '/App_Plugins/uTransporter/HelpDialog.html',
                callback: function (confirmed) {
                    if (!confirmed) {
                        return;
                    }
                }
            });
        };

        $scope.downloadLog = function () {
            $(".spinner").hide();

            uTransporterService.downloadLog();
        };

        $scope.dryRun = function () {
            $(".spinner").hide();

            dialogService.open({
                template: '/App_Plugins/uTransporter/ConfirmActionDryrun.html?t=' + new Date().getTime(),
                callback: function (confirmed) {

                    if (!confirmed) {
                        return;
                    }

                    $scope.dryRunStyle = "";
                    $(".spinner").show("slow");

                    uTransporterService.dryRun().then(function (dryRunResult) {
                        $(".spinner").hide("slow");

                        if (dryRunResult.successful) {
                            $scope.dryRunStyle = "dryrun-success";
                            $scope.dryRunResult = dryRunResult;
                            $scope.dryRunMessage = dryRunResult.message;
                        } else {
                            $scope.dryRunStyle = "dryrun-failed";
                            $scope.dryRunMessage = dryRunResult.message;
                            $scope.dryRunText = "Please consult the Umbraco Sync log for more information.";
                        }

                        // Give the server some time to write to log file, 2.5s seems ok.
                        if ($scope.showLog) {
                            setTimeout(function () {
                                $scope.getLog();
                            }, 2500);
                        }
                    });
                }
            });
        }
    });