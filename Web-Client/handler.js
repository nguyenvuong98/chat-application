var socket;
var username;
var contentChat = [];
var connected = [];

function connect(name){
    if(name == username)return;
    console.log(connected);
    let connect = connected.find(x => x == name);
    if( connect != null || connect != undefined){
        alert('Bạn đã kết nối với '+name+'.');
        return;
    }
    socket.emit('request-connect', name);
}
function sendMessage(roomName){
    let id = '#chat-' + roomName;
    let input = $(id);
    socket.emit('request-chat', {Username: username, Content: input.val(), RoomName:  roomName });
    input.val('');
}
$(document).ready(() => {
    $("#btn-login").click(() => {
        username = $("#username").val();
        if(username == '' || username == null || username == undefined){
            alert("Tên đăng nhập không được để trống!");
            return;
        }
        if ( username.includes(' ')) {
            alert("Tên đăng nhập không được có khoảngs trống!");
            return;
        }
        $.ajax({url :"http://localhost:3000/sign-in",
                method: "post",
                data:{username: username}
                }).done((res) => {
                    if(res.status){
                        $('#login').css('display','none');
                        $('#login-success').css('display','block');
                        $('#d-username').html(username);
                        socket = io("http://localhost:3000");
                        socket.emit("username",username);
                        socket.on("users", (data) => {
                            var table = $("#users");
                            var content = '';
                            data.forEach(item => {
                                if(item.username == username)   return;
                                var online = item.isOnline ? "bg-success": "bg-secondary";
                                content +=`<tr>
                                    <td>${item.username}</td>
                                    <td>
                                        <div style="width:15px;height: 15px; border-radius: 50%" class="${online}"></div>
                                    </td>
                                    <td><button class="btn-send-message" onclick="connect('${item.username}')">Click</button></td>
                                </tr>`;
                            });
                            table.html(content);
                        });
                        init();

                    }
                    else{
                        alert(res.message);
                    }
                });
    });
    
    let init = () => {
        socket.on("await-connect", data => {
            roomName = data.room;
            var confirm = window.confirm(data.name + " muốn kết nối");
            if(confirm){
                console.log(confirm);
                socket.emit("accept-connect",{friend: data.name, room: roomName});
            }
            else{
                console.log(confirm);
                socket.emit("reject-connect",{username: username, friend: data.name});
            }
        })
        socket.on("my-conversation", data => {
            let element = $('#'+data.roomName);
            let style = data.username == username? 'is-me' : 'is-friend';
            let content = `
                <div>
                    <div class="${style}">
                        <span style="font-size: small; margin-bottom: 5px; display: block; color:#7b6c6c">${data.username}</span>
                        <span class="content content-bg">${data.content}</span>
                    </div>
                    <div class="clearfix"></div>
                </div>
            `;
            element.append(content);
            $("."+data.roomName).animate({ scrollTop: $("."+data.roomName).height() }, 100);
        })
        socket.on('connect-success', (data) => {
            console.log(data);
            let chat = {friend: data.friend, roomName: data.roomName };
            contentChat.push(chat);
            connected.push(data.friend);
            connected.push(data.username);
            let parent = $('.users-chat');
            let title = data.friend == username? data.username : data.friend;
            let chatbox = `
                <div class="chat-box">
                    <div class="chat-box-header">
                        <p class="title">${title}</p>
                    </div>
                    <div class="chat-box-content ${data.roomName}">
                        <div id="${data.roomName}"></div>
                    </div>
                    <div class="chat-box-footer">
                        <div class="txt-input p-2">
                                <input type="text" class="txt" id="chat-${data.roomName}"/>
                        </div>
                        <div class="submit p-2">
                            <button class="btn-send-message" onclick="sendMessage('${data.roomName}')">OK</button>
                        </div>
                    </div>
                </div>
            `;
            parent.append(chatbox);
           
        });
        socket.on('rejected-connect', data => {
            alert(data.friend +' từ chối kết nối!');
        });
    };
    
})