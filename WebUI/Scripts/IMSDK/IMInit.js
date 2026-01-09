if (_CustomerId > 0 && _IMUserID != "" && _IMUSerPD != "") {

    layui.use('layim', function (layim) {
        //基础配置
        layim.config({
            //初始化接口
            init: {
                url: _RootPath + "SPWebAPI/SPIM/GetIMUserInit?customerId=" + _CustomerId + "&token=" + _Token
              , data: {}
            }
            //查看群员接口
            //, members: {
            //    url: 'json/getMembers.json'
            //  , data: {}
            //}

            //上传图片接口
          , uploadImage: {
              url: _RootPath + "SPWebAPI/SPIM/UploadImage?token=" + _Token //（返回的数据格式见下文）
            , type: 'POST' //默认post
          }

            //上传文件接口
          , uploadFile: {
              url: _RootPath + "SPWebAPI/SPIM/UploadFile?token=" + _Token //（返回的数据格式见下文）
            , type: '' //默认post
          }

            //扩展工具栏
            //, tool: [{
            //    alias: 'audio'
            //  , title: '发送音频'
            //  , icon: '&#xe6fc;'
            //}]

            //,brief: true //是否简约模式（若开启则不显示主面板）

            , title: '众销通' //自定义主面板最小化时的标题
            , right: '20px' //主面板相对浏览器右侧距离
            , minRight: '10px' //聊天面板最小化时相对浏览器右侧距离
            , initSkin: '2.jpg' //1-5 设置初始背景
            //,skin: ['aaa.jpg'] //新增皮肤
            //,isfriend: false //是否开启好友
            , isgroup: false //是否开启群组
            , min: true //是否始终最小化主面板，默认false
            , notice: true //是否开启桌面消息提醒，默认false
            , voice: false //声音提醒，默认开启，声音文件为：default.wav

            //, msgbox: layui.cache.dir + 'css/modules/layim/html/msgbox.html' //消息盒子页面地址，若不开启，剔除该项即可
            //, find: layui.cache.dir + 'css/modules/layim/html/find.html' //发现页面地址，若不开启，剔除该项即可
          , chatLog: layui.cache.dir + 'css/modules/layim/html/MessageHistory.aspx' //聊天记录页面地址，若不开启，剔除该项即可

        });

        /*
        layim.chat({
          name: '在线客服-小苍'
          ,type: 'kefu'
          ,avatar: 'http://tva3.sinaimg.cn/crop.0.0.180.180.180/7f5f6861jw1e8qgp5bmzyj2050050aa8.jpg'
          ,id: -1
        });
        layim.chat({
          name: '在线客服-心心'
          ,type: 'kefu'
          ,avatar: 'http://tva1.sinaimg.cn/crop.219.144.555.555.180/0068iARejw8esk724mra6j30rs0rstap.jpg'
          ,id: -2
        });
        layim.setChatMin();*/

        //监听在线状态的切换事件
        layim.on('online', function (data) {
            //console.log(data);
            var status = (data == "online") ? 1 : 0;
            $.ajax({
                url: _RootPath + "SPWebAPI/SPIM/UpdateIMUserStatus?customerId=" + _CustomerId + "&status=" + status + "&token=" + _Token,
                type: "GET",
                data: null,
                success: function (data) {

                },
                error: function (data) {
                    //alert(data);
                }
            });

        });

        //监听签名修改
        layim.on('sign', function (value) {
            //console.log(value);
            $.ajax({
                url: _RootPath + "SPWebAPI/SPIM/UpdateIMUserSign?customerId=" + _CustomerId + "&sign=" + value + "&token=" + _Token,
                type: "GET",
                data: null,
                success: function (data) {

                },
                error: function (data) {
                    //alert(data);
                }
            });
        });

        //监听自定义工具栏点击，以添加代码为例
        //layim.on('tool(audio)', function (insert) {
        //    layui.use('upload', function () {
        //        var upload = layui.upload;

        //        //执行实例
        //        var uploadInst = upload.render({
        //            elem: '#test1' //绑定元素
        //          , url: _RootPath + "SPWebAPI/SPIM/UploadAudio?token=" + _Token //上传接口
        //          , done: function (res) {
        //              //上传完毕回调
        //          }
        //          , error: function () {
        //              //请求异常回调
        //          }
        //        });
        //    });
        //});

        //监听layim建立就绪
        layim.on('ready', function (res) {

            //console.log(res.mine);

            //layim.msgbox(1); //模拟消息盒子有新消息，实际使用时，一般是动态获得

            //添加好友（如果检测到该socket）
            //layim.addList({
            //    type: 'group'
            //  , avatar: "http://tva3.sinaimg.cn/crop.64.106.361.361.50/7181dbb3jw8evfbtem8edj20ci0dpq3a.jpg"
            //  , groupname: 'Angular开发'
            //  , id: "12333333"
            //  , members: 0
            //});
            //layim.addList({
            //    type: 'friend'
            //  , avatar: "http://tp2.sinaimg.cn/2386568184/180/40050524279/0"
            //  , username: '冲田杏梨'
            //  , groupid: 2
            //  , id: "1233333312121212"
            //  , remark: "本人冲田杏梨将结束AV女优的工作"
            //});

        });

        //监听发送消息
        layim.on('sendMessage', function (data) {
            //var To = data.to;
            //console.log(data);

            //if (To.type === 'friend') {
            //    layim.setChatStatus('<span style="color:#FF5722;">对方正在输入。。。</span>');
            //}
            //判断是否为音频,有需要的时候再写
            //var audio
            //data.mine.content
            var sendPrivateText = function () {
                var id = conn.getUniqueId();                 // 生成本地消息id
                var msg = new WebIM.message('txt', id);      // 创建文本消息
                msg.set({
                    msg: data.mine.content,                  // 消息内容
                    to: data.to.id,                          // 接收消息对象（用户id）
                    roomType: false,
                    success: function (id, serverMsgId) {
                        //console.log('send private text Success');
                        //保存到DB
                        $.ajax({
                            url: _RootPath + "SPWebAPI/SPIM/UpdateIMMessage?messageFrom=" + data.mine.id + "&messageTo=" + data.to.id + "&content=" + data.mine.content + "&token=" + _Token,
                            type: "POST",
                            data: null,
                            success: function (data) {

                            },
                            error: function (data) {
                                //alert(data);
                            }
                        });
                    },
                    fail: function (e) {
                        //console.log("Send private text error");
                    }
                });
                msg.body.chatType = 'singleChat';
                conn.send(msg.body);
                //console.log(data.mine.content);
            };
            sendPrivateText();

        });

        //监听查看群员
        layim.on('members', function (data) {
            //console.log(data);
        });

        //监听聊天窗口的切换
        layim.on('chatChange', function (res) {

        });



    });


    var conn = new WebIM.connection({
        isMultiLoginSessions: WebIM.config.isMultiLoginSessions,
        https: typeof WebIM.config.https === 'boolean' ? WebIM.config.https : location.protocol === 'https:',
        url: WebIM.config.xmppURL,
        heartBeatWait: WebIM.config.heartBeatWait,
        autoReconnectNumMax: WebIM.config.autoReconnectNumMax,
        autoReconnectInterval: WebIM.config.autoReconnectInterval,
        apiUrl: WebIM.config.apiURL,
        isAutoLogin: true
    });

    conn.listen({
        onOpened: function (message) {          //连接成功回调
            // 如果isAutoLogin设置为false，那么必须手动设置上线，否则无法收消息
            // 手动上线指的是调用conn.setPresence(); 如果conn初始化时已将isAutoLogin设置为true
            // 则无需调用conn.setPresence();                
        },
        onClosed: function (message) { },         //连接关闭回调
        onTextMessage: function (message) {

            var fromId = message.from;
            var fromName = "";
            var fromAvatar = "";
            var fromContent = message.data;

            var friendArr = layui.layim.cache().friend[0].list;
            var isExists = false;
            for (var i = 0; i < friendArr.length; i++) {
                if (fromId.toLowerCase() == friendArr[i].id.toLowerCase()) {
                    fromId = friendArr[i].id;
                    fromName = friendArr[i].username;
                    fromAvatar = friendArr[i].avatar;
                    isExists = true;
                    break;
                }
            }
            if (!isExists) {
                $.ajax({
                    url: _RootPath + "SPWebAPI/SPIM/GetIMuserByIMId?IMId=" + fromId + "&token=" + _Token,
                    type: "GET",
                    data: null,
                    success: function (data) {
                        if (data.Flag == 1) {
                            var res = new Object();
                            res.type = 'friend'; //列表类型，只支持friend和group两种
                            res.avatar = data.Result.avatar; //好友头像
                            res.username = data.Result.username; //好友昵称
                            res.groupid = data.Result.groupid; //所在的分组id
                            res.id = data.Result.id; //好友id
                            res.sign = data.Result.sign;//好友签名
                            //添加好友
                            layui.layim.addList(res);

                            layui.layim.getMessage({
                                username: res.username
                                , avatar: res.avatar
                                , id: fromId
                                , type: "friend"
                                , content: fromContent
                            });

                        }
                    },
                    error: function (data) {
                        //alert(data);
                    }
                });

            } else {
                layui.layim.getMessage({
                    username: fromName
                    , avatar: fromAvatar
                    , id: fromId
                    , type: "friend"
                    , content: fromContent
                });
            }
        },    //收到文本消息
        onEmojiMessage: function (message) {

        },   //收到表情消息
        onPictureMessage: function (message) {

        }, //收到图片消息
        onCmdMessage: function (message) {

        },     //收到命令消息
        onAudioMessage: function (message) {

        },   //收到音频消息
        onLocationMessage: function (message) {

        },//收到位置消息
        onFileMessage: function (message) {

        },    //收到文件消息
        onVideoMessage: function (message) {
            var node = document.getElementById('privateVideo');
            var option = {
                url: message.url,
                headers: {
                    'Accept': 'audio/mp4'
                },
                onFileDownloadComplete: function (response) {
                    var objectURL = WebIM.utils.parseDownloadResponse.call(conn, response);
                    node.src = objectURL;
                },
                onFileDownloadError: function () {
                    console.log('File down load error.')
                }
            };
            WebIM.utils.download.call(conn, option);
        },   //收到视频消息
        onPresence: function (message) { },       //处理“广播”或“发布-订阅”消息，如联系人订阅请求、处理群组、聊天室被踢解散等消息
        onRoster: function (message) { },         //处理好友申请
        onInviteMessage: function (message) { },  //处理群组邀请
        onOnline: function () { },                  //本机网络连接成功
        onOffline: function () { },                 //本机网络掉线
        onError: function (message) { },          //失败回调
        onBlacklistUpdate: function (list) {       //黑名单变动
            // 查询黑名单，将好友拉黑，将好友从黑名单移除都会回调这个函数，list则是黑名单现有的所有好友信息
            console.log(list);
        },
        onReceivedMessage: function (message) { },    //收到消息送达服务器回执
        onDeliveredMessage: function (message) { },   //收到消息送达客户端回执
        onReadMessage: function (message) { },        //收到消息已读回执
        onCreateGroup: function (message) { },        //创建群组成功回执（需调用createGroupNew）
        onMutedMessage: function (message) { }        //如果用户在A群组被禁言，在A群发消息会走这个回调并且消息不会传递给群其它成员
    });

    var options = {
        apiUrl: WebIM.config.apiURL,
        user: _IMUserID,
        pwd: _IMUSerPD,
        appKey: WebIM.config.appkey
    };
    conn.open(options);
}


function AddNewChat(friendId) {

    if (_CustomerId < 1 || _Token == "") {
        bootbox.dialog({
            message: "请先登录再进行操作！",
            buttons:
            {
                "Confirm":
                {
                    "label": "确定",
                    "className": "btn-sm btn-primary",
                    "callback": function () {
                        window.location.href = "Login.aspx";
                    }
                }
            }
        });
    } else {
        layui.use('layim', function (layim) {            
            $.ajax({
                url: _RootPath + "SPWebAPI/SPIM/AddIMFriend?friendCustomerId=" + friendId + "&token=" + _Token,
                type: "POST",
                data: null,
                success: function (data) {
                    if (data.Flag == 1) {
                        var res = new Object();
                        res.type = 'friend'; //列表类型，只支持friend和group两种
                        res.avatar = data.Result.avatar; //好友头像
                        res.username = data.Result.username; //好友昵称
                        res.groupid = data.Result.groupid; //所在的分组id
                        res.id = data.Result.id; //好友id
                        res.sign = data.Result.sign;//好友签名

                        //先判断是否在列表中
                        var friendArr = layim.cache().friend[0].list;
                        var isExists = false;
                        for (var i = 0; i < friendArr.length; i++) {
                            if (res.id.toLowerCase() == friendArr[i].id.toLowerCase()) {                                
                                isExists = true;
                                break;
                            }
                        }
                        if (!isExists) {
                            //添加好友
                            layim.addList(res);
                        }
                        //显示聊天窗口
                        layim.chat({
                            name: res.username //名称
                          , type: 'friend' //聊天类型
                          , avatar: res.avatar //头像
                          , id: res.id //好友id
                        })

                    }
                },
                error: function (data) {
                    //alert(data);
                }
            });
        });
    }
    return false;
}