import '../App.css';
import searchIcon from './search-icon.png';
import { Component } from 'react';

export class SearchBar extends Component {
  render() {
    return (
      <div className="header-container">
        <form className="search-form"
          onSubmit={this.props.onSubmitHandler}>
          <input className="search-bar"
            name="q"
            type="text"
            placeholder="Buscar producto"
            onChange={this.props.onChangeHandler} />
          <input className="search-button"
            type="image"
            alt="Search"
            src={searchIcon} />
        </form>
      </div>
    );
  }
};

export class Filters extends Component {
  render() {
    return (
      <form className="filters-container">
        <label className="filters-field">Categoría:</label>
        <input className="filters-input"
          name="category"
          type="text"
          list="categories"
          onChange={this.props.onChangeHandler} />
        <datalist id="categories">
          {this.props.categories.map(
            (category, index) => <option key={index} value={category.name} />
          )}
        </datalist>
        <label className="filters-field">Marca:</label>
        <input className="filters-input"
          name="brand"
          type="text"
          list="brands"
          onChange={this.props.onChangeHandler} />
        <datalist id="brands">
          {this.props.brands.map(
            (brand, index) => <option key={index} value={brand.name} />
          )}
        </datalist>
        <label className="filters-field">Precio:</label>
        <input className="filters-price"
          name="minPrice"
          type="number"
          placeholder="$ Mínimo"
          step="0.01"
          onChange={this.props.onChangeHandler} />
        <input className="filters-price"
          name="maxPrice"
          type="number"
          placeholder="$ Máximo"
          step="0.01"
          onChange={this.props.onChangeHandler} />
      </form>
    );
  }
};
