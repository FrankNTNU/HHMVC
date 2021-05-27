# HealthHelperMVC專案儲存庫
>使用說明
+ 該MVC專案沿用BLL、DTO、DAO架構，皆為類別庫(class library)
    * 每個資料表皆需要BLL類別
    + DTO屬性可自行擴充
    - DAO實作與資料庫的互動
+ 後臺專案置於UI/Areas/Admin，並使用底下Content/View/Shared的AdminLayout.cshtml模板，後臺控制器(Controller)與檢視(View)也在底下定義
+ Admin/Views/Shared/MessageForm.cshtml定義訊息顯示內容，在該動作檢視中加入鑲入訊息顯示
`@Html.Partial("~/Areas/Admin/Views/Shared/MessageForm.cshtml")`
+ 各表單的定義皆至於View>所屬檢視資料夾>Partial，使用範例為如下，此片段程式碼需置於該動作的檢視頁面中:
`@using (Html.BeginForm(actionName, controllerName, FormMethod.Post, new { @enctype = "multipart/form-data" }))
 {
     @Html.Partial("Partial/xxxForm")
 }`
+ 該MVC專案使用Azure上HealthHelper資料庫，加入實體模型後在DAL>HHContext設定實體模型變數
