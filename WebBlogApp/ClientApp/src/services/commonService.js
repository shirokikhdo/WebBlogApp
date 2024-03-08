export const ACCOUNT_URL = 'account';
export const PROFILE_URL = '/profile';
export const LOGIN_URL = '/login';
const USERS_URL = 'users';
export const NEWS_URL = 'news';
const BASE_URL = 'login';
const TOKEN_NAME = 'Token';

function sendRequest(url, successAction, errorAction) {
    fetch(url)
        .then(response => {
            if (response.status === 401) {
                window.location.href = BASE_URL;
            } else {
              successAction();
            }
        })
        .catch(error => {
            errorAction();
        });
}

export async function getToken(login, password) {
    const url = ACCOUNT_URL + '/token';
    const token = await sendAuthenticatedRequest(url, 'POST', login, password);
    localStorage.setItem(TOKEN_NAME, token.accessToken);
    window.location.href = PROFILE_URL;
}

async function sendAuthenticatedRequest(url, method, username, password, data) {
    var headers = new Headers();
    headers.set('Authorization', 'Basic' + btoa(username + ':' + password));
    if (data) {
        headers.set('Content-Type', 'application/json');
    }
    var requestOptions = {
        method: method,
        headers: headers,
        body: data ? JSON.stringify(data) : undefined
    };
    var resultFetch = await fetch(url, requestOptions);
    if (resultFetch.ok) {
        var result = await resultFetch.json();
        return result;
    } else {
        throw new Error('Ошибка ' + resultFetch.status + ':' + resultFetch.statusText);
    }
}

export async function sendRequestWithToken(url, method, data) {
    var headers = new Headers();
    var token = localStorage.getItem(TOKEN_NAME);
    headers.set('Authorization', `Bearer ${token}`);
    if (data) {
        headers.set('Content-Type', 'application/json');
    }
    var requestOptions = {
        method: method,
        headers: headers,
        body: data ? JSON.stringify(data) : undefined
    };
    var resultFetch = await fetch(url, requestOptions);
    if (resultFetch.ok) {
        var result = await resultFetch.json();
        return result;
    } else {
        errorRequest(resultFetch.status)
    }
}

function errorRequest(status) {
    if (status === 401) {
        window.location.href = BASE_URL;
    }
}