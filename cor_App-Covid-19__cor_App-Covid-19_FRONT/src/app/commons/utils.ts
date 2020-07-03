export const getAvatarFromName = (name: string): string => {
  if (name) {
    let firstLetters: string[];
    if (name.includes(',')) {
      firstLetters = name
        .split(',')
        .map(word => word.trim()[0])
        .reverse();
    } else if (name.trim().includes(' ')) {
      firstLetters = name.split(' ').map(word => word[0]);
    } else {
      return name
        .trim()
        .slice(0, 2)
        .toUpperCase();
    }

    return firstLetters.length > 1 ? `${firstLetters[0].toUpperCase()}${firstLetters[1].toUpperCase()}` : undefined;
  }
  return undefined;
};

type IPassportFrontColors = 'green' | 'red' | 'grey';

export const passportColorMapper = (color: string): IPassportFrontColors => {
  color = color ? color.toLowerCase() : null;
  if (color === 'rojo') {
    return 'red';
  } else if (color === 'verde') {
    return 'green';
  } else {
    return 'grey';
  }
};
