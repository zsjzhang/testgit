
//vcyberapp.factory('TreeData', function () {

//    function TreeData(tree, cbIsSame) {
//        var _this = this;
//        this.tree = tree;
//        this.isSame = cbIsSame || function (item1, item2) { return item1 === item2; };
//        /**
//         * 折叠/展开
//         * @param item {Object}
//         * @param folded
//         * @private
//         */
//        this._fold = function (item, folded) {
//            item.folded = folded;
//        };
//        /**
//         * 折叠指定的节点
//         * @param item {Object}
//         */
//        this.fold = function (item) {
//            this._fold(item, true);
//        };
//        /**
//         * 展开指定的节点
//         * @param item {Object}
//         */
//        this.unfold = function (item) {
//            this._fold(item, false);
//        };
//        /**
//         * 切换节点的折叠状态
//         * @param item {Object}
//         */
//        this.toggleFold = function (item) {
//            this._fold(item, !item.folded);
//        };
//        /**
//         * 检查指定节点的折叠状态
//         * @param item {Object}
//         * @returns {boolean}
//         */
//        this.isFolded = function (item) {
//            return item.folded;
//        };
//        /**
//         * 递归检查指定节点是否有选中状态的子节点，不检查当前节点状态
//         * @param item {Object} 起始节点
//         * @return {boolean}
//         */
//        this.hasCheckedChildren = function (item) {
//            if (item.items == null) {
//                return true;
//            }
//            for (var i = 0; i < item.items.length; i++) {
//                if (item.items[i].checked) {
//                    return true;
//                }
//            }
//            return false;
//        };
//        /**
//         * 递归检查指定节点是否有未选中状态的子节点，不检查当前节点状态
//         * @param item {Object} 起始节点
//         * @return {boolean}
//         */
//        this.hasUncheckedChildren = function (item) {
//            if (item.items == null) {
//                return false;
//            }
//            for (var i = 0; i < item.items.length; i++) {
//                if (!item.items[i].checked) {
//                    return true;
//                }
//            }
//            return false;
//        };
//        /**
//         * 指定节点是否半选状态，但不检查当前节点。即：既有被选中的子节点，也有未选中的子节点
//         * @param item {Object} 起始节点
//         * @return {boolean}
//         */
//        this.hasSemiCheckedChildren = function (item) {
//            return this.hasCheckedChildren(item) && this.hasUncheckedChildren(item);
//        };
//        /**
//         * 当前节点是否半选状态，hasSemiCheckedChildren的别名
//         * @param item {Object}
//         * @returns {boolean}
//         */
//        this.isSemiChecked = function (item) {
//            return this.hasSemiCheckedChildren(item);
//        };
//        /**
//         * 更新item的父级节点，重新检查它们的checked和semiChecked状态
//         * @param items
//         * @param item
//         * @private
//         */
//        this._updateParents = function (items, item) {
//            if (items != null) {
//                $.each(items, function (subItem) {
//                    if (_this.hasChildren(items[subItem], item)) {
//                        // 先要递归更新子级，否则中间节点的状态可能仍然处于选中状态，会影响当前节点的判断
//                        //_this._updateParents(_this.tree, items[subItem]);
//                        _this._updateParents(items[subItem].items, item);
//                        items[subItem].checked =  !_this.hasUncheckedChildren(items[subItem]);
//                        //items[subItem].semiChecked = _this.isSemiChecked(items[subItem]);
//                        items[subItem].semiChecked = true;
//                    }
//                });
//            }
//        };
//        this.updateChecked = function (item) {
//            this._updateParents(this.tree, item);
//        };
//        /**
//         * 选中/反选指定节点
//         * @param item {Object}
//         * @param checked {boolean}
//         * @private
//         */
//        this._check = function (item, checked) {
//            this._check1(item, checked);
//            this._updateParents(this.tree, item);
//        };
//        this._check1 = function (item, checked) {
//            item.checked = checked;
//            // 把当前节点的选中状态应用到所有下级
//            if (item.items != null) {
//                $.each(item.items, function (subItem) {
//                    _this._check1(item.items[subItem], checked);
//                });
//            }

//            // 自动更新所有上级的状态
//            //this._updateParents(this.tree, item);
//        };
//        this._find = function (items, item) {
//            if (!items)
//                return null;
//            // 在子节点中查找
//            for (var i = 0; i < items.length; ++i) {
//                var subItem = items[i];
//                // 如果找到了则直接返回
//                if (this.isSame(subItem, item))
//                    return subItem;
//                // 否则递归查找
//                var subResult = _this._find(subItem.items, item);
//                if (subResult)
//                    return subResult;
//            }
//            return null;
//        };
//        /**
//         * 查找指定的节点，会使用cbIsSame参数
//         * @param item
//         * @returns {Object}
//         */
//        this.find = function (item) {
//            return this._find(this.tree, item);
//        };
//        /**
//         * parent及其子节点中有没有指定的subItem节点
//         * @param parent {Object}
//         * @param subItem {Object|Array}
//         * @returns {boolean}
//         */
//        this.hasChildren = function (parent, subItem) {
//            if (parent != null) {
//                if (parent.items != null && parent.items.length > 0) {
//                    return _this._find(parent.items, subItem);
//                }
//            }
//            return false;

//        };
//        /**
//         * 选中节点
//         * @param item {Object}
//         * @param checked {boolean}
//         */
//        this.check = function (item, checked) {
//            item = this.find(item);
//            this._check(item, checked || angular.isUndefined(checked));
//        };
//        /**10
//         * 反选节点
//         * @param item {Object}
//         */
//        this.uncheck = function (item) {
//            item = this.find(item);
//            this._check(item, false);
//        };
//        /**
//         * 切换节点的选中状态
//         * @param item {Object}
//         */
//        this.toggleCheck = function (item) {
//            item = this.find(item);
//            this._check(item, !item.checked);
//        };
//        /**
//         * 指定节点是否被选中
//         * @param item {Object}
//         * @returns {boolean}checked
//         */
//        this.isChecked = function (item) {
//            item = this.find(item);
//            return item.checked;
//        };
//    }
//    return TreeData;
//});

vcyberapp.factory('TreeData', function () {

    function TreeData(tree, cbIsSame) {
        var _this = this;
        this.tree = tree;
        this.isSame = cbIsSame || function (item1, item2) { return item1 === item2; };
        /**
         * 折叠/展开
         * @param item {Object}
         * @param folded
         * @private
         */
        this._fold = function (item, folded) {
            item.folded = folded;
        };
        /**
         * 折叠指定的节点
         * @param item {Object}
         */
        this.fold = function (item) {
            this._fold(item, true);
        };
        /**
         * 展开指定的节点
         * @param item {Object}
         */
        this.unfold = function (item) {
            this._fold(item, false);
        };
        /**
         * 切换节点的折叠状态
         * @param item {Object}
         */
        this.toggleFold = function (item) {
            this._fold(item, !item.folded);
        };
        /**
         * 检查指定节点的折叠状态
         * @param item {Object}
         * @returns {boolean}
         */
        this.isFolded = function (item) {
            return item.folded;
        }; 
        /**
         * 递归检查指定节点是否有选中状态的子节点，不检查当前节点状态
         * @param item {Object} 起始节点
         * @return {boolean}
         */
        this.hasCheckedChildren = function (item) {
            if (item.data == null) {
                return true;
            }
            for (var i = 0; i < item.data.length; i++) {
                if (item.data[i].IsChecked) {
                    return true;
                }
            }
            return false;
        };
        /**
         * 递归检查指定节点是否有未选中状态的子节点，不检查当前节点状态
         * @param item {Object} 起始节点
         * @return {boolean}
         */
        this.hasUncheckedChildren = function (item) {
            if (item.data == null) {
                return false;
            }
            for (var i = 0; i < item.data.length; i++) {
                if (!item.data[i].IsChecked) {
                    return true;
                }
            }
            return false;
        };
        /**
         * 指定节点是否半选状态，但不检查当前节点。即：既有被选中的子节点，也有未选中的子节点
         * @param item {Object} 起始节点
         * @return {boolean}
         */
        this.hasSemiCheckedChildren = function (item) {
            return this.hasCheckedChildren(item) && this.hasUncheckedChildren(item);
        };
        /**
         * 当前节点是否半选状态，hasSemiCheckedChildren的别名
         * @param item {Object}
         * @returns {boolean}
         */
        this.isSemiChecked = function (item) {
            return this.hasSemiCheckedChildren(item);
        };
        /**
         * 更新item的父级节点，重新检查它们的checked和semiChecked状态
         * @param items
         * @param item
         * @private
         */
        this._updateParents = function (data, item) {
            if (data != null) {
                $.each(data, function (subItem) {
                    if (_this.hasChildren(data[subItem], item)) {
                        // 先要递归更新子级，否则中间节点的状态可能仍然处于选中状态，会影响当前节点的判断
                        //_this._updateParents(_this.tree, items[subItem]);
                        _this._updateParents(data[subItem].data, item);
                        data[subItem].IsChecked = !_this.hasUncheckedChildren(data[subItem]);
                        //items[subItem].semiChecked = _this.isSemiChecked(items[subItem]);
                        data[subItem].semiChecked = true;
                    }
                });
            }
        };
        this.updateChecked = function (item) {
            this._updateParents(this.tree, item);
        };
        /**
         * 选中/反选指定节点
         * @param item {Object}
         * @param checked {boolean}
         * @private
         */
        this._check = function (item, IsChecked) {
            this._check1(item, IsChecked);
            this._updateParents(this.tree, item);
        };
        this._check1 = function (item, IsChecked) {
            item.IsChecked = IsChecked;
            // 把当前节点的选中状态应用到所有下级
            if (item.data != null) {
                $.each(item.data, function (subItem) {
                    _this._check1(item.data[subItem], IsChecked);
                });
            }

            // 自动更新所有上级的状态
            //this._updateParents(this.tree, item);
        };
        this._find = function (data, item) {
            if (!data)
                return null;
            // 在子节点中查找
            for (var i = 0; i < data.length; ++i) {
                var subItem = data[i];
                // 如果找到了则直接返回
                if (this.isSame(subItem, item))
                    return subItem;
                // 否则递归查找
                var subResult = _this._find(subItem.data, item);
                if (subResult)
                    return subResult;
            }
            return null;
        };
        /**
         * 查找指定的节点，会使用cbIsSame参数
         * @param item
         * @returns {Object}
         */
        this.find = function (item) {
            return this._find(this.tree, item);
        };
        /**
         * parent及其子节点中有没有指定的subItem节点
         * @param parent {Object}
         * @param subItem {Object|Array}
         * @returns {boolean}
         */
        this.hasChildren = function (parent, subItem) {
            if (parent != null) {
                if (parent.data != null && parent.data.length > 0) {
                    return _this._find(parent.data, subItem);
                }
            }
            return false;

        };
        /**
         * 选中节点
         * @param item {Object}
         * @param checked {boolean}
         */
        this.check = function (item, IsChecked) {
            item = this.find(item);
            this._check(item, IsChecked || angular.isUndefined(IsChecked));
        };
        /**10
         * 反选节点
         * @param item {Object}
         */
        this.uncheck = function (item) {
            item = this.find(item);
            this._check(item, false);
        };
        /**
         * 切换节点的选中状态
         * @param item {Object}
         */
        this.toggleCheck = function (item) {
            item = this.find(item);
            this._check(item, !item.IsChecked);
        };
        /**
         * 指定节点是否被选中
         * @param item {Object}
         * @returns {boolean}
         */
        this.isChecked = function (item) {
            item = this.find(item);
            return item.IsChecked;
        };
    }
    return TreeData;
});
