﻿document.addEventListener('submit', e => {
    //console.log(e);
    const form = e.target;
    if (form.id == 'room-form') {
        e.preventDefault();
        let formData = new FormData(form);
        fetch("/api/room", {
            method: 'POST',
            body: formData
        }).then(r => {
            console.log(r);
            if (r.status == 201) {
                window.location.reload();
            }
            else {
                r.text().then(alert);
            }
        });

    }
    if (form.id == 'category-form') {
        e.preventDefault();
        let formData = new FormData(form);
        ctgId = formData.get("category-id");
        if (ctgId) {
            console.log("Оновлення категорії" + ctgId);
        }
        else {
            console.log("Додавання нової категорії");
        }
    }
})
document.addEventListener('DOMContentLoaded', function () {
    const authButton = document.getElementById("auth-button");
    if (authButton) authButton.addEventListener('click', authButtonClick);

    const confirmEmailButton = document.getElementById("confirm-email-button");
    if (confirmEmailButton) confirmEmailButton.addEventListener('click', confirmEmailClick);

    initAdminPage();
    serveReserveButtons();
    serveAdminButtons();
});

function confirmEmailClick() {
    const emailCodeInput = document.getElementById("email-code");
    if (!emailCodeInput) throw "Element '#email-code' not found";
    const emailMessage = document.getElementById("email-message");
    if (!emailMessage) throw "Element '#email-message' not found";

    const code = emailCodeInput.value?.trim();
    if (!code) {
        emailMessage.classList.remove('visually-hidden');
        emailMessage.innerText = "Необхідно ввести код";
    }
    const email = emailCodeInput.getAttribute("data-email");

    fetch(`/api/auth?email=${email}&code=${code}`, { method: 'PATCH' })
        .then(r => {
            if (r.status === 202) {
                window.location.reload();
            }
            else {
                emailMessage.classList.remove('visually-hidden');
                emailMessage.innerText = "Код не прийнято";
            }
        })
}

function serveAdminButtons() {
    for (let btn of document.querySelectorAll('[data-type="edit-category"]')) {
        btn.addEventListener('click', e => {
            let b = e.target.closest('[data-type="edit-category"]');
            document.querySelector('[name="category-id"]').value =
                b.getAttribute("data-category-id");
            document.querySelector('[name="category-name"]').value =
                b.getAttribute("data-category-name");
            document.querySelector('[name="category-description"]').value =
                b.getAttribute("data-category-description");
            document.querySelector('[name="category-slug"]').value =
                b.getAttribute("data-category-slug");
        });
    }
    for (let btn of document.querySelectorAll('[data-type="delete-category"]')) {
        btn.addEventListener('click', e => {
            e.stopPropagation();
            let b = e.target.closest('[data-type="delete-category"]');
            let id = b.getAttribute("data-category-id");
            if (id) {
                if (confirm("Ви підтверджуєте видання категорії?")) {
                    fetch(`/api/category/${id}`, { method: 'DELETE' }).then(r => {
                        if (r.status < 400) {
                            window.location.reload();
                        }
                        else {
                            alert("Виникла помилка видалення");
                        }
                    })
                }
            }
            else {
                alert("Помилка розмітки - немає id елемента");
            }
        });
    }

    for (let btn of document.querySelectorAll('[data-type="restore-category"]')) {
        btn.addEventListener('click', e => {
            let b = e.target.closest('[data-type="restore-category"]');
            let id = b.getAttribute("data-category-id");
            if (id) {
                if (confirm("Ви підтверджуєте відновлення категорії?")) {
                    fetch(`/api/category?id=${id}`, { method: 'RESTORE' }).then(r => {
                        if (r.status < 400) {
                            //window.location.reload();
                            r.text().then(console.log);
                        }
                        else {
                            alert("Виникла помилка відновлення");
                        }
                    })
                }
                else {
                    alert("Помилка розмітки - немає id елемента");
                }
            }
        });
    }
}
function authButtonClick() {
    const authEmail = document.getElementById("auth-email");
    if (!authEmail) throw "Element '#auth-email' not found";
    const authPassword = document.getElementById("auth-password");
    if (!authPassword) throw "Element '#auth-password' not found";
    const authMessage = document.getElementById("auth-message");
    if (!authMessage) throw "Element '#auth-message' not found";

    const email = authEmail.value.trim();
    if (email == "") {
        authMessage.classList.remove('visually-hidden');
        authMessage.innerHTML = "Необхідно ввести E-mail";
        return;
    }

    const password = authPassword.value;

    fetch(`/api/auth?e-mail=${email}&password=${password}`)
        .then(r => {
            if (r.status != 200) {
                authMessage.classList.remove('visually-hidden');
                authMessage.innerText = "Вхід скасовано, перевірте введені дані"
            }
            else {
                window.location.reload();
            }
        });
}

////////

function initAdminPage() {
    loadCategories();
}
function loadCategories() {
    const conteiner = document.getElementById("category-container");
    if (!conteiner) return;
    fetch("/api/category")
        .then(r => r.json())
        .then(j => {
            let html = "";
            for (let ctg of j) {
                html += `<p data-id="${ctg["id"]}" onclick="ctgClick('${ctg["id"]}')">${ctg["name"]}</p>`;
            }
            html += `Назва: <input id="ctg-name" /><br/>
                Опис: <textarea  id="ctg-description"></textarea><br/>
                Фото: <input type="file" id="ctg-photo" /<br/>
                <button onclick='addCategory()'>+</button>`;

            conteiner.innerHTML = html;
        })
}
function ctgClick(ctgId) {
    fetch("/api/location/" + ctgId)
        .then(r => r.json())
        .then(j => {
            const container = document.getElementById("location-container");
            let html = "";
            for (let loc of j) {
                html += `<p data-id="${loc["id"]}" onclick="locClick(event)">${loc["name"]}</p>`;
            }
            html += `Назва: <input id="loc-name" /><br/>
            Опис: <textarea  id="loc-description"></textarea><br/>
            Рейтинг: <input id="loc-stars" type="number" /><br/>
            Фото: <input id="loc-photo" type="file" /><br/>
            <button onclick='addLocation("${ctgId}")'>+</button>`;
            container.innerHTML = html;
        });
}
function addLocation(ctgId) {
    const ctgName = document.getElementById("loc-name").value;
    const ctgDescription = document.getElementById("loc-description").value;
    const ctgStars = document.getElementById("loc-stars").value;
    const locPhoto = document.getElementById("loc-photo");

    if (confirm(`Додаємо локацію ${ctgName} ${ctgDescription} ${ctgStars} ?`)) {
        let formData = new FormData();
        formData.append("categoryId", ctgId);
        formData.append("name", ctgName);
        formData.append("description", ctgDescription);
        formData.append("stars", ctgStars);
        formData.append("photo", locPhoto.files[0]);
        fetch("/api/location", {
            method: 'POST',
            body: formData
        })
            .then(r => {
                console.log(r);
                if (r.status == 201) {
                    ctgClick(ctgId);
                }
                else {
                    alert("error");
                }
            });
    }
}
function addCategory() {
    const ctgName = document.getElementById("ctg-name").value;
    const ctgDescription = document.getElementById("ctg-description").value;
    const ctgPhoto = document.getElementById("ctg-photo");
    if (confirm(`Додаємо категорію ${ctgName} ${ctgDescription} ?`)) {
        let formData = new FormData();
        formData.append("name", ctgName);
        formData.append("description", ctgDescription);
        formData.append("photo", ctgPhoto.files[0]);
        fetch("/api/category", {
            method: 'POST',

            body: formData
        })
            .then(r => {
                console.log(r)
                if (r.status == 201) {
                    loadCategories();
                }
                else {
                    alert("error")
                }
            });
    }
}

function serveReserveButtons() {
    for (let btn of document.querySelectorAll('[data-type="reserve-room"]')) {
        btn.addEventListener('click', e => {
            const cont = e.target.closest('[data-type="reserve-room"]');
            const roomId = cont.getAttribute('data-room-id');
            const userId = cont.getAttribute('data-user-id');
            const date = cont.getAttribute('data-date');
            console.log(roomId, userId, date);
            fetch('/api/room/reserve', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({
                    date,
                    roomId,
                    userId
                })
            }).then(r => {
                if (r.status === 201) {
                    window.location.reload();
                }
                else {
                    r.text().then(alert);
                }
            });
        });
    }

    for (let btn of document.querySelectorAll('[data-type="drop-reserve-room"]')) {
        btn.addEventListener('click', e => {
            const cont = e.target.closest('[data-type="drop-reserve-room"]');
            const reserveId = cont.getAttribute('data-reserve-id');
            if (!confirm("Підтверджуєте скасування замовлення " + reserveId)) {
                return;
            }
            fetch('/api/room/reserve?reserveId=' + reserveId, {
                method: 'DELETE'
            }).then(r => {
                if (r.status === 202) {
                    window.location.reload();
                }
                else {
                    r.text().then(alert);
                }
            });
        });
    }
}