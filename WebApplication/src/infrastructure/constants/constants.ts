export const RegExps = {
  EMAIL_REGEX: '[A-Za-z0-9._%+-]{2,}@[a-zA-Z-,0-9]{1,}([.]{1}[a-zA-Z, 0-9]{2,}|[.]{1}[a-zA-Z, 0-9, -]{2,}[.]{1}[a-zA-Z, -]{2,})(|[.]{1}[a-zA-Z, -]{2,}[.]{1}[a-zA-Z, -]{2,})',
  PASSWORD_REGEX: '^(?=.*[a-z])(?=.*[0-9])(?=.*[A-Z]).{6,}$',
  LATIN_NAMES_REGEX: '^[a-zA-Z, -]+$',
  NUMBER_REGEX: '^[0-9]+',
  CYRILLIC_NAMES_REGEX: "^[аАбБвВгГдДеЕжЖзЗиИйЙкКлЛмМнНоОпПрРсСтТуУфФхХцЦчЧшШщЩьъЪюЮяЯ, -.]+$",
  LATIN_AND_CYRILLIC_NAMES_REGEX: "^[A-Za-zаАбБвВгГдДеЕжЖзЗиИйЙкКлЛмМнНоОпПрРсСтТуУфФхХцЦчЧшШщЩьъЪюЮяЯ, -]+$",
  PHONE_NUMBER_REGEX: "^[0-9, +-]+$",
  LETTERS_AND_NUMBERS_REGEX: "^[A-Za-zаАбБвВгГдДеЕжЖзЗиИйЙкКлЛмМнНоОпПрРсСтТуУфФхХцЦчЧшШщЩьъЪюЮяЯ,0-9, -.]+$",
  DIPLOMA_NUMBER_REGEX: "^[A-Za-zаАбБвВгГдДеЕжЖзЗиИйЙкКлЛмМнНоОпПрРсСтТуУфФхХцЦчЧшШщЩьъЪюЮяЯ,0-9,-]+$",
  LATIN_AND_NUMBER_REGEX: "^[a-zA-Z,0-9,-/]+$",
  LATIN_AND_NUMBER_ONLY_REGEX: "^[a-zA-Z, 0-9]+$"
};

export const UserRoleAliases = {
  ADMINISTRATOR: 'administrator',
  CONTROL_USER: 'controlUser',
  UNIVERSITY_USER: 'universityUser',
  LOT_RESULT_USER: 'commitUser',
  RESULT_SIGNER_USER: 'resultSignerUser'
};

export const ContentTypes = {
  EXCEL: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet',
  PDF: 'application/pdf'
};

export const EducationalQualificationAliases = {
  PROFESSIONALBACHELOR: 'ProfessionalBachelor',
  BACHELOR: 'Bachelor',
  MASTERWITHSECONDARY: 'MasterWithSecondary',
  MASTERWITHBACHELOR: 'MasterWithBachelor',
  DOCTORAL: 'Doctoral',
  TRAINEE: 'Trainee'
}

export const DiplomaTypeAliases = {
  SECONADARY: 'Secondary',
  BACHELOR: 'Bachelor',
  MASTER: 'Master',
}

//old email regex: [^@]+@[^@]+\.[a-zA-Z]{2,}
