import './app.css';
import { Component } from 'react';
import { BrowserRouter, Routes, Route } from 'react-router-dom';

import Home from './views/home';
import ProductView from './views/product-view';

class App extends Component {
  render() {
    return (
      <BrowserRouter>
        <Routes>
          <Route path="/" element={<Home />} />
          <Route path="/product/:id" element={<ProductView />} />
        </Routes>
      </BrowserRouter>
    );
  }
};

export default App;
