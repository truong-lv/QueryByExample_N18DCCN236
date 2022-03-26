var stringNotifi = `You have installed DevExpress Products in Evaluation Mode - To purchase a license, please visit us online at: <a style="color:#303030" rel="nofollow" href="http://www.devexpress.com/purchase">www.devexpress.com/purchase</a>.<br>If you've purchased DevExpress Products and need to register your license, please review: <a style="color:#303030" rel="nofollow" href="http://www.devexpress.com/Support/Center/KB/p/K18106.aspx">www.devexpress.com/Support/Center/KB/p/K18106.aspx</a>.`
setTimeout(() => {
    var notifiEle = $("div>table>tbody>tr>td")
    notifiEle.each((index, ele) => {
        if ($(ele).html() === stringNotifi) {
            /*console.log($(ele).parents("div"))*/
            $(ele).parents("div").remove();
        }
    })
    
}, 100)

