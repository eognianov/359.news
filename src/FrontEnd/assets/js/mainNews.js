const mainNewsService = (() => {
	function gatLastMainNews(take, loaded) {
		return kinvey.get('appdata', 'mainNews', 'kinvey', take, loaded);
	}

	return {
		gatLastMainNews
	};
})();