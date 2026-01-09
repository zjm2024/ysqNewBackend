$(function () {
    onPageInit();
});

var grid = new JGrid();
function onPageInit() {
    grid.jqGrid.ID = "CardNewsList";
    grid.jqGrid.PagerID = "CardNewsListDiv";
    grid.jqGrid.Source = "../Common/GetDataJsonGrid.ashx";
    grid.jqGrid.Params = "table=FarmGameViewList";
    grid.jqGrid.LengthMenu = ["20", "50", "100", "150"];   
    grid.jqGrid.DefaultSortCol = "CreatedAt";
    grid.jqGrid.DefaultSort = "desc";
    grid.jqGrid.Multiselect = true;
    grid.jqGrid.AddHidColumn("FarmGameID");
    grid.jqGrid.AddColumn("HeaderLogo", "玩家头像", true, "center", 50,
        function (obj, options, rowObject) {
            return "<img src='" + obj + "' style='height:50px;'></div>";
        });
    grid.jqGrid.AddColumn("CustomerName", "玩家昵称", true, "center", 50);
    grid.jqGrid.AddColumn("Gold", "金币", true, "center", 20);
    grid.jqGrid.AddColumn("Diamond", "钻石", true, "center", 20);
    grid.jqGrid.AddColumn("Water", "水滴", true, "center", 20);
    grid.jqGrid.AddColumn("Fertilizer", "肥料", true, "center", 20);

    for (var i = 1; i <= 8; i++) {
        grid.jqGrid.AddColumn("FieldsType" + i, "土地" + i, true, "center", 20,
        function (obj, options, rowObject) {
            if (obj == "Carrot_seed")
                return "萝卜<div style='color:#f30000;'>成长中</div>";
            else if (obj == "Tomato_seed")
                return "番茄<div style='color:#f30000;'>成长中</div>";
            else if (obj == "Eggplant_seed")
                return "茄子<div style='color:#f30000;'>成长中</div>";
            else if (obj == "Apple_seed")
                return "苹果<div style='color:#f30000;'>成长中</div>";
            else if (obj == "Carrot_ripening")
                return "萝卜<div style='color:#03d513;'>已成熟</div>";
            else if (obj == "Tomato_ripening")
                return "番茄<div style='color:#03d513;'>已成熟</div>";
            else if (obj == "Eggplant_ripening")
                return "茄子<div style='color:#03d513;'>已成熟</div>";
            else if (obj == "Apple_ripening")
                return "苹果<div style='color:#03d513;'>已成熟</div>";
            else if (obj == "")
                return "空地";
            else if (obj == "Lock")
                return "未解锁";
            else
                return obj;
        }, false);
    }
    

    grid.jqGrid.AddColumn("CreatedAt", "开玩时间", true, "center", 50);
    grid.jqGrid.CreateTable();   
}
