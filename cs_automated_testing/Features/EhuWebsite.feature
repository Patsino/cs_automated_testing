Feature: EHU Website Functionality
  Verify the main functionalities of the EHU website.

  Scenario: Navigate to About Page
    Given I am on the homepage
    When I click the About button
    Then I should see the About page with the correct title and header

  Scenario: Perform a Search
    Given I am on the homepage
    When I search for "study programs"
    Then I should see search results containing "study program"

  Scenario: Switch Language to Lithuanian
    Given I am on the homepage
    When I switch the language to Lithuanian
    Then the URL should be "https://lt.ehu.lt/"
    And the page's lang attribute should be "lt-LT"
