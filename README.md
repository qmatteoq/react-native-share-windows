# react-native-share-windows

## Getting started

`$ npm install react-native-share-windows --save`

### Mostly automatic installation

`$ react-native link react-native-share-windows`

## Usage

```javascript
import ReactNativeShareWindows from 'react-native-share-windows';
```

```javascript
const share = async () => {
    await ReactNativeShareWindows.share("This is the title, "https://www.website.com");
}
```