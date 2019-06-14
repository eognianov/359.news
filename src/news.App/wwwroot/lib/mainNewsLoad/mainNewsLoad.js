$(() => {
    let moreNewsBtn = $('#mainNewsLoad');
    let mainNewsList = $('#mainNewsPosts > ul.posts');

    let imgLazyLoad = new LazyLoad({
        elements_selector: "img.lozad",
        load_delay: 2000
        // Assign the callbacks defined above
    });

    let iframeLazyLoad = new LazyLoad({
        elements_selector: "iframe.lozad",
        // Assign the callbacks defined above
    });

    let contentLazyLoad = new LazyLoad({
        elements_selector: ('section.lozad'),

        callback_loaded: function (element) {
            console.log("Finished Section", element);
            
        },

        callback_reveal: function (element) {
            console.log("Reveal ", element);
            console.log("Elements", $(element).find('.lozad'));
            let elementsToLoad = $(element).find('.lozad').length;
            console.log(elementsToLoad);
            
            let loadedElements = $(element).find('.lozad .loaded').length;
            console.log(loadedElements);
            
            },

        
    });

    loadMainNews();

    moreNewsBtn.on('click', loadMainNews);

    function addNews(element) {
        let newNews = $(`<li><article><header><h3></h3><time></time></header><a class="image"><img class="lozad" data-src=${element.ImageUrl} alt></a></article></li>`);
        newNews.find('h3').html(element.Title);
        newNews.find('time').html(element.CreatedOn);
        newNews.find('a.image').attr({ href: element.OriginalUrl, target: "_blank" });
        newNews.find('img').data('src', element.ImageUrl);
        mainNewsList.append(newNews);
    }
			
    function loadMainNews() {
        let loadedNews = $('#mainNewsPosts > ul.posts li').length;
        moreNewsBtn.attr("disabled", true).addClass("btnLoading", {
            complete: mainNewsService.gatLastMainNews(5, loadedNews).then((mainNews) => {
                mainNews.map((element) => addNews(element), moreNewsBtn.removeAttr("disabled").removeClass('btnLoading'));
            }).then(() => { imgLazyLoad.update();}).catch(function (err) {
                console.log(err);
            })
        });
    }
});