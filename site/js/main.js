// Функция для навигации на мобильных устройствах
document.addEventListener('DOMContentLoaded', function() {
  const navToggle = document.querySelector('.nav-toggle');
  const mainNav = document.querySelector('.main-nav');

  navToggle.addEventListener('click', function() {
    mainNav.classList.toggle('active');
    navToggle.classList.toggle('active');
  });

  // Скрывать меню при клике на ссылку
  const navLinks = document.querySelectorAll('.main-nav a');
  navLinks.forEach(link => {
    link.addEventListener('click', function() {
      mainNav.classList.remove('active');
      navToggle.classList.remove('active');
    });
  });

  // Изменение цвета шапки при прокрутке
  const header = document.querySelector('.header');
  
  window.addEventListener('scroll', function() {
    if (window.scrollY > 50) {
      header.classList.add('scrolled');
    } else {
      header.classList.remove('scrolled');
    }
  });

  // Табы для секции геймплея
  const tabButtons = document.querySelectorAll('.tab-btn');
  const tabPanes = document.querySelectorAll('.tab-pane');

  tabButtons.forEach(button => {
    button.addEventListener('click', function() {
      // Удаляем активный класс со всех кнопок и панелей
      tabButtons.forEach(btn => btn.classList.remove('active'));
      tabPanes.forEach(pane => pane.classList.remove('active'));
      
      // Добавляем активный класс к текущей кнопке
      this.classList.add('active');
      
      // Показываем соответствующую панель
      const tabId = this.getAttribute('data-tab');
      document.getElementById(tabId).classList.add('active');
    });
  });

  // Заполнение контента вкладок
  fillTabContent();
});

// Функция для заполнения контента вкладок геймплея
function fillTabContent() {
  // Контент для вкладки "Выживание"
  const survivalTab = document.getElementById('survival');
  if (survivalTab) {
    survivalTab.innerHTML = `
      <div class="gameplay-content">
        <div class="gameplay-text">
          <h3>Выживание</h3>
          <p>Игроку предстоит адаптироваться к новым условиям, исследуя мир и удовлетворяя базовые потребности:</p>
          <ul>
            <li>Еда и вода: Найти ресурсы питания, которые могут быть ограничены в зависимости от биома.</li>
            <li>Защита: Сражаться с хищниками или убегать от них, используя скрытность и маневренность.</li>
            <li>Укрытие: Создавать временные убежища или использовать природные условия для защиты от хищников и погодных явлений.</li>
          </ul>
        </div>
        <div class="gameplay-image">
          <img src="https://i.ibb.co/hFhrPmGw/image.jpg" alt="Выживание" class="rounded-image">
        </div>
      </div>
    `;
  }

  // Контент для вкладки "Адаптация и эволюция"
  const evolutionTab = document.getElementById('evolution');
  if (evolutionTab) {
    evolutionTab.innerHTML = `
      <div class="gameplay-content">
        <div class="gameplay-text">
          <h3>Адаптация и эволюция</h3>
          <p>При каждом перерождении игрок может модифицировать свое существо:</p>
          <ul>
            <li>Выбирать положительные модификации (например, крепкие кости, острые когти, быстрый метаболизм).</li>
            <li>Балансировать улучшения с помощью негативных изменений (снижение скорости, плохое зрение, повышенная уязвимость).</li>
            <li>Участвовать в естественном отборе, который влияет на доступные мутации.</li>
          </ul>
          <p>Помимо биологической эволюции, существо может осваивать элементы технологического развития, включая создание простых инструментов и использование природных ресурсов.</p>
        </div>
        <div class="gameplay-image">
          <img src="https://i.ibb.co/tMv4Gz1N/image.jpg" alt="Эволюция" class="rounded-image">
        </div>
      </div>
    `;
  }

  // Контент для вкладки "Взаимодействие с миром"
  const interactionTab = document.getElementById('interaction');
  if (interactionTab) {
    interactionTab.innerHTML = `
      <div class="gameplay-content">
        <div class="gameplay-text">
          <h3>Взаимодействие с миром</h3>
          <p>Игрок может:</p>
          <ul>
            <li>Исследовать: Узнавать новые виды существ, растений и их свойства (ядовитость, лечебные эффекты, возможность использования для мутаций).</li>
            <li>Симбиоз: Заключать условные "союзы" с определенными существами для совместного выживания.</li>
            <li>Манипулировать экосистемой: Выживание одного вида может зависеть от игрока, создавая непредсказуемые последствия.</li>
          </ul>
        </div>
        <div class="gameplay-image">
          <img src="https://i.ibb.co/mrDzFHzF/image.png" alt="Взаимодействие с миром" class="rounded-image">
        </div>
      </div>
    `;
  }

  // Контент для вкладки "Развитие мира"
  const worldDevTab = document.getElementById('world-dev');
  if (worldDevTab) {
    worldDevTab.innerHTML = `
      <div class="gameplay-content">
        <div class="gameplay-text">
          <h3>Развитие мира</h3>
          <p>Мир вокруг игрока тоже меняется:</p>
          <ul>
            <li>Эволюция экосистемы: Виды флоры и фауны приспосабливаются к действиям игрока.</li>
            <li>Циклы жизни: Животные и растения размножаются, стареют и умирают, меняя ландшафт.</li>
            <li>Климатические и сезонные изменения: Например, суровые зимы, которые требуют тепла и запасов.</li>
          </ul>
        </div>
        <div class="gameplay-image">
          <img src="https://i.ibb.co/tTpYSLZQ/image.jpg" alt="Развитие мира" class="rounded-image">
        </div>
      </div>
    `;
  }

  // Контент для вкладки "Смерть и перерождение"
  const rebirthTab = document.getElementById('rebirth');
  if (rebirthTab) {
    rebirthTab.innerHTML = `
      <div class="gameplay-content">
        <div class="gameplay-text">
          <h3>Смерть и перерождение</h3>
          <p>После смерти игрок может:</p>
          <ul>
            <li>Переродиться в том же мире, используя "яйцо" или потомство, оставленное ранее.</li>
            <li>Переродиться в новом мире с полностью случайной генерацией.</li>
          </ul>
          <p>При перерождении в потомстве сохраняются некоторые черты и улучшения, выбранные игроком.</p>
        </div>
        <div class="gameplay-image">
          <img src="https://i.ibb.co/s9BrjdVY/image.png" alt="Смерть и перерождение" class="rounded-image">
        </div>
      </div>
    `;
  }
}