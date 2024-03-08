import React, { Component } from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layout';
import { Home } from './components/Home';
import { FetchData } from './components/FetchData';
import { Counter } from './components/Counter';
import './custom.css'
import { LOGIN_URL, PROFILE_URL } from './services/commonService';
import Login from './components/users/Login';
import UserProfile from './components/users/UserProfile';

export default class App extends Component {
  static displayName = App.name;

  render () {
    return (
      <Layout>
        <Route exact path='/' component={Home} />
        <Route path='/counter' component={Counter} />
        <Route path='/fetch-data' component={FetchData} />
        <Route path={LOGIN_URL} component={Login} />
        <Route path={PROFILE_URL} component={UserProfile} />
      </Layout>
    );
  }
}
