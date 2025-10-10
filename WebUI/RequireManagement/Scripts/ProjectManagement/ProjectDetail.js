$(function () {
    //var projectId = GetQueryString("projectId");
    //GetProjectDetil(projectId);
});

function GetProjectDetil(projectId) {
    GetData("GetProjectSite", projectId, function (data) {
        var titleObj = $("#lblTitle");
        var businessNameObj = $("#lblBusinessName");
        var commissionObj = $("#lblCommission");
        var agencyNameObj = $("#lblAgencyName");
        var endDateObj = $("#lblEndDate");
        var descriptionObj = $("#divDescription");

        var projectVO = data.Result;

        titleObj.html(projectVO.Title);
        businessNameObj.html(projectVO.BusinessName);
        commissionObj.html(projectVO.Commission);
        agencyNameObj.html(projectVO.AgencyName);
        endDateObj.html(new Date(projectVO.EndDate).format("yyyy-MM-dd"));
       
        GetData("GetRequireSite", projectVO.RequirementId, function (data) {
            
            var requireVO = data.Result;

            descriptionObj.html(requireVO.Description);

        }, function (data) {
            //load_hide();
        });

    }, function (data) {        
        //load_hide();
    });
}