import searchIcon from './search-icon.png';
import './search-bar.css';
import { Component } from 'react';

class SearchBar extends Component {
  render() {
    return (
      <form className="search-bar"
        onSubmit={this.props.onSubmitHandler}>
        <input className="search-input"
          name="q"
          type="text"
          placeholder="Buscar producto"
          onChange={this.props.onChangeHandler} />
        <input className="search-button"
          type="image"
          alt="Search"
          src={searchIcon} />
      </form>
    );
  }
};

export default SearchBar;
